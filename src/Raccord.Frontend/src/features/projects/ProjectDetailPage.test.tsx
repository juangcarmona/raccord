import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { afterEach, describe, expect, it, vi } from "vitest";
import type { Schemas } from "../../shared/api/client";
import { json, renderAt, stubApi } from "../../test/render";
import { ProjectDetailPage } from "./ProjectDetailPage";

const line = (id: string, description: string, billingState: string): Schemas["BillableLineResponse"] => ({
    id,
    kind: "equipment",
    description,
    quantity: 1,
    unitPrice: 100,
    discountPercent: 0,
    vatRate: 21,
    netAmount: 100,
    billingState,
    draftId: null,
    invoiceId: null,
});

const project = (billableLines: Schemas["BillableLineResponse"][]): Schemas["ProjectResponse"] => ({
    id: "p1",
    name: "Rodaje campaña primavera",
    customerId: "c1",
    customerName: "Productora Ejemplo S.A.",
    startDate: "2026-09-14",
    endDate: "2026-09-18",
    currency: "EUR",
    billableLines,
    status: "draft",
    hasCancellationWarning: false,
    cancelledAt: null,
    externalReference: null,
    contactPersonId: null,
});

const routes = [
    { path: "/projects/:id", element: <ProjectDetailPage /> },
    { path: "/drafts/:id", element: <p>Página de borrador</p> },
];
const customerResponse = [
    { id: "c1", legalName: "Productora Ejemplo S.A." },
    { id: "c2", legalName: "Productora Alternativa S.A." },
];
const customerApi = {
    "GET /api/customers": () => json(customerResponse),
    "GET /api/contacts/c1/people": () => json([]),
    "GET /api/contacts/c2/people": () =>
        json([
            {
                id: "person-c2",
                contactId: "c2",
                name: "Production lead",
                email: "lead@example.test",
                phone: null,
                isDefault: true,
                isAdministrative: false,
            },
        ]),
    "GET /api/projects/p1/periods": () => json([]),
};

describe("ProjectDetailPage", () => {
    afterEach(() => {
        vi.unstubAllGlobals();
    });

    it("only lets unbilled lines be selected for a draft", async () => {
        stubApi({
            ...customerApi,
            "GET /api/projects/p1": () =>
                json(project([line("a", "Cámara", "unbilled"), line("b", "Óptica", "invoiced")])),
        });

        renderAt("/projects/p1", routes);

        expect(await screen.findByRole("checkbox", { name: "Seleccionar «Cámara»" })).toBeEnabled();
        expect(screen.getByRole("checkbox", { name: "Seleccionar «Óptica»" })).toBeDisabled();
        expect(screen.getByText("Facturada")).toBeInTheDocument();
        expect(screen.getByRole("button", { name: "Crear borrador con 0 línea(s)" })).toBeDisabled();
    });

    it("creates a draft from the selected lines and opens it", async () => {
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () =>
                json(project([line("a", "Cámara", "unbilled"), line("b", "Gaffer", "unbilled")])),
            "POST /api/invoice-drafts": () => json({ id: "d9" }, 201),
        });
        const router = renderAt("/projects/p1", routes);
        const user = userEvent.setup();

        await user.click(await screen.findByRole("checkbox", { name: "Seleccionar «Gaffer»" }));
        await user.click(screen.getByRole("button", { name: "Crear borrador con 1 línea(s)" }));

        await waitFor(() => {
            expect(router.state.location.pathname).toBe("/drafts/d9");
        });
        expect(requests.find((request) => request.method === "POST")?.body).toEqual({
            projectId: "p1",
            billableLineIds: ["b"],
            seriesCode: null,
        });
    });

    it("says when a project has nothing to bill", async () => {
        stubApi({ ...customerApi, "GET /api/projects/p1": () => json(project([])) });

        renderAt("/projects/p1", routes);

        expect(await screen.findByText("Este proyecto no tiene líneas facturables.")).toBeInTheDocument();
    });

    it("changes a project's lifecycle status", async () => {
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () => json(project([])),
            "PUT /api/projects/p1/status": () => json({ ...project([]), status: "confirmed" }),
        });

        renderAt("/projects/p1", routes);
        const user = userEvent.setup();
        await user.selectOptions(await screen.findByRole("combobox", { name: "Estado" }), "confirmed");
        await user.click(screen.getByRole("button", { name: "Guardar" }));

        expect(requests.find((request) => request.method === "PUT")?.body).toEqual({ status: "confirmed" });
    });

    it("saves edits to a project's name, period and external reference", async () => {
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () => json(project([])),
            "PUT /api/projects/p1": () => json(project([])),
        });

        renderAt("/projects/p1", routes);
        const user = userEvent.setup();
        await user.clear(await screen.findByRole("textbox", { name: "Nombre del proyecto" }));
        await user.type(screen.getByRole("textbox", { name: "Nombre del proyecto" }), "Rodaje verano");
        await user.clear(screen.getByLabelText("Inicio"));
        await user.type(screen.getByLabelText("Inicio"), "2026-10-01");
        await user.clear(screen.getByLabelText("Fin"));
        await user.type(screen.getByLabelText("Fin"), "2026-10-03");
        await user.type(screen.getByLabelText(/Referencia externa/), "PO-2026-041");
        await user.click(screen.getByRole("button", { name: "Guardar datos" }));

        expect(requests.find((request) => request.method === "PUT")?.body).toEqual({
            name: "Rodaje verano",
            customerId: "c1",
            startDate: "2026-10-01",
            endDate: "2026-10-03",
            externalReference: "PO-2026-041",
            projectTypeId: null,
            teamId: null,
            contactPersonId: null,
        });
    });

    it("updates the selected project customer", async () => {
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () => json(project([])),
            "PUT /api/projects/p1": () => json(project([])),
        });

        renderAt("/projects/p1", routes);
        const user = userEvent.setup();
        await user.selectOptions(await screen.findByRole("combobox", { name: "Cliente" }), "c2");
        await user.selectOptions(await screen.findByRole("combobox", { name: "Persona de contacto" }), "person-c2");
        await user.click(screen.getByRole("button", { name: "Guardar datos" }));

        expect(requests.find((request) => request.method === "PUT")?.body).toMatchObject({
            customerId: "c2",
            contactPersonId: "person-c2",
        });
    });

    it("confirms cancellation after showing the open-work warning", async () => {
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () => json(project([])),
            "GET /api/projects/p1/cancellation-preview": () =>
                json({
                    openTaskCount: 2,
                    plannedCrewCount: 0,
                    plannedTransportCount: 0,
                    activeInvitationCount: 0,
                    canCompleteOpenTasks: true,
                }),
            "POST /api/projects/p1/cancellation": () =>
                json({ ...project([]), status: "cancelled", hasCancellationWarning: true }),
        });

        renderAt("/projects/p1", routes);
        const user = userEvent.setup();
        await user.click(await screen.findByRole("button", { name: "Cancelar proyecto" }));
        expect(await screen.findByRole("alert")).toHaveTextContent(
            "Quedan 2 tareas abiertas, 0 personas asignadas y 0 vehículos asignados.",
        );
        await user.click(screen.getByRole("checkbox", { name: "Completar 2 tareas abiertas" }));
        await user.click(screen.getByRole("button", { name: "Confirmar cancelación" }));

        expect(requests.find((request) => request.method === "POST")?.body).toEqual({
            confirm: true,
            completeOpenTasks: true,
            removePlannedResources: false,
            withdrawInvitations: false,
        });
    });

    it("reports a missing project instead of an empty page", async () => {
        stubApi({});

        renderAt("/projects/p1", routes);

        expect(await screen.findByRole("alert")).toHaveTextContent("No existe o ya no está disponible.");
    });
});
