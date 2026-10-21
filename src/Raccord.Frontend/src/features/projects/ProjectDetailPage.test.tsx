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
    projectNumber: "10001",
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
    accountManagerId: null,
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
    "GET /api/project-coordination/types": () => json([{ id: "type-1", name: "Commercial" }]),
    "GET /api/project-coordination/teams": () => json([{ id: "team-1", name: "Camera team" }]),
    "GET /api/projects/account-managers": () => json([{ id: "manager-1", displayName: "Office Manager" }]),
    "GET /api/projects/p1/periods": () => json([]),
    "GET /api/projects/p1/context": () => json({ projectId: "p1", entries: [], history: [] }),
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

    it("saves edits to a project's name, number, period and external reference", async () => {
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () => json(project([])),
            "PUT /api/projects/p1": () => json(project([])),
        });

        renderAt("/projects/p1", routes);
        const user = userEvent.setup();
        await user.clear(await screen.findByRole("textbox", { name: "Nombre del proyecto" }));
        await user.type(screen.getByRole("textbox", { name: "Nombre del proyecto" }), "Rodaje verano");
        await user.clear(screen.getByRole("textbox", { name: "Número de proyecto" }));
        await user.type(screen.getByRole("textbox", { name: "Número de proyecto" }), "PRJ-2026-041");
        await user.clear(screen.getByLabelText("Inicio"));
        await user.type(screen.getByLabelText("Inicio"), "2026-10-01");
        await user.clear(screen.getByLabelText("Fin"));
        await user.type(screen.getByLabelText("Fin"), "2026-10-03");
        await user.type(screen.getByLabelText(/Referencia externa/), "PO-2026-041");
        await user.selectOptions(await screen.findByRole("combobox", { name: "Tipo de proyecto" }), "type-1");
        await user.selectOptions(screen.getByRole("combobox", { name: "Equipo" }), "team-1");
        await user.click(screen.getByRole("button", { name: "Guardar datos" }));

        expect(requests.find((request) => request.method === "PUT")?.body).toEqual({
            name: "Rodaje verano",
            projectNumber: "PRJ-2026-041",
            customerId: "c1",
            startDate: "2026-10-01",
            endDate: "2026-10-03",
            externalReference: "PO-2026-041",
            projectTypeId: "type-1",
            teamId: "team-1",
            contactPersonId: null,
            accountManagerId: null,
        });
    });

    it("assigns and saves an eligible project account manager", async () => {
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () => json(project([])),
            "PUT /api/projects/p1": () => json(project([])),
        });

        renderAt("/projects/p1", routes);
        const user = userEvent.setup();
        await user.selectOptions(await screen.findByRole("combobox", { name: "Responsable de cuenta" }), "manager-1");
        await user.click(screen.getByRole("button", { name: "Guardar datos" }));

        expect(requests.find((request) => request.method === "PUT")?.body).toMatchObject({
            accountManagerId: "manager-1",
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

    it("adds project notes and files and reloads them in the detail view", async () => {
        const entries: { id: string; kind: string; name: string; content: string; createdAt: string }[] = [];
        const requests = stubApi({
            ...customerApi,
            "GET /api/projects/p1": () => json(project([])),
            "GET /api/projects/p1/context": () => json({ projectId: "p1", entries, history: [] }),
            "POST /api/projects/p1/context/notes": async (request) => {
                const body = (await request.json()) as { title: string; body: string };
                entries.push({
                    id: "n1",
                    kind: "note",
                    name: body.title,
                    content: body.body,
                    createdAt: "2026-10-21T00:00:00Z",
                });
                return json({ id: "n1" }, 201);
            },
            "POST /api/projects/p1/context/files": async (request) => {
                const file = (await request.json()) as { name: string; content: string };
                entries.push({
                    id: "f1",
                    kind: "file",
                    name: file.name,
                    content: file.content,
                    createdAt: "2026-10-21T00:00:00Z",
                });
                return json({ id: "f1" }, 201);
            },
        });

        renderAt("/projects/p1", routes);
        const user = userEvent.setup();
        await user.type(await screen.findByRole("textbox", { name: "Título de la nota" }), "Acceso al recinto");
        await user.type(screen.getByRole("textbox", { name: "Contenido de la nota" }), "Usar entrada lateral.");
        await user.click(screen.getByRole("button", { name: "Añadir nota" }));
        expect(await screen.findByText("Acceso al recinto")).toBeInTheDocument();
        await user.type(screen.getByRole("textbox", { name: "Nombre del archivo" }), "Plano de acceso");
        await user.type(screen.getByRole("textbox", { name: "Contenido del archivo" }), "plano-v1.pdf");
        await user.click(screen.getByRole("button", { name: "Añadir archivo" }));
        expect(await screen.findByText("Plano de acceso")).toBeInTheDocument();

        expect(requests.filter((request) => request.method === "POST").map((request) => request.path)).toEqual([
            "/api/projects/p1/context/notes",
            "/api/projects/p1/context/files",
        ]);
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
