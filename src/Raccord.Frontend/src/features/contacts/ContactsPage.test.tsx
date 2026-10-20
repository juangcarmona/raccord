import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { json, renderAt, stubApi } from "../../test/render";
import { ContactFormPage } from "./ContactFormPage";
import { ContactsPage } from "./ContactsPage";

const company = {
    id: "contact-1",
    type: "Company",
    contactNumber: "10001",
    legalName: "Northlight Films",
    taxId: "A12345678",
    address: null,
    email: "billing@northlight.example",
    roles: ["Customer", "Supplier"],
    isInvoiceable: false,
};

describe("contacts directory", () => {
    it("shows the empty state and create action", async () => {
        stubApi({
            "GET /api/contacts": () =>
                json({ items: [], page: 1, pageSize: 10, totalCount: 0, hasNextPage: false, hasPreviousPage: false }),
        });
        renderAt("/contacts", [{ path: "/contacts", element: <ContactsPage /> }]);

        expect(await screen.findByText("Todavía no hay contactos.")).toBeInTheDocument();
        expect(screen.getByRole("link", { name: "Nuevo contacto" })).toHaveAttribute("href", "/contacts/new");
    });

    it("loads and edits an existing contact without losing its assigned number", async () => {
        const requests = stubApi({
            "GET /api/contacts/contact-1": () => json(company),
            "PUT /api/contacts/contact-1": () => json({ ...company, legalName: "Northlight Updated" }),
        });
        renderAt("/contacts/contact-1", [{ path: "/contacts/:id", element: <ContactFormPage /> }]);

        const name = await screen.findByLabelText("Razón social");
        expect(name).toHaveValue("Northlight Films");
        expect(screen.getByLabelText(/Número de contacto/)).toHaveValue("10001");
        await userEvent.clear(name);
        await userEvent.type(name, "Northlight Updated");
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        await waitFor(() => {
            expect(requests).toHaveLength(2);
        });
        expect(requests[1]?.body).toMatchObject({
            legalName: "Northlight Updated",
            contactNumber: "10001",
            roles: ["Customer", "Supplier"],
        });
    });

    it("shows and retries the directory error state", async () => {
        let calls = 0;
        stubApi({
            "GET /api/contacts": () => {
                calls += 1;
                return calls === 1
                    ? json({ title: "Unavailable" }, 503)
                    : json({
                          items: [],
                          page: 1,
                          pageSize: 10,
                          totalCount: 0,
                          hasNextPage: false,
                          hasPreviousPage: false,
                      });
            },
        });
        renderAt("/contacts", [{ path: "/contacts", element: <ContactsPage /> }]);

        expect(await screen.findByRole("alert")).toBeInTheDocument();
        await userEvent.click(screen.getByRole("button", { name: "Reintentar" }));
        expect(await screen.findByText("Todavía no hay contactos.")).toBeInTheDocument();
    });

    it("filters a paged directory by role and shows the next page", async () => {
        const requests = stubApi({
            "GET /api/contacts": (request) => {
                const url = new URL(request.url);
                const isSupplier = url.searchParams.get("role") === "Supplier";
                return json({
                    items: [company],
                    page: Number(url.searchParams.get("page") ?? 1),
                    pageSize: 10,
                    totalCount: isSupplier ? 1 : 20,
                    hasNextPage: !isSupplier && url.searchParams.get("page") !== "2",
                    hasPreviousPage: url.searchParams.get("page") === "2",
                });
            },
        });
        renderAt("/contacts", [{ path: "/contacts", element: <ContactsPage /> }]);

        expect(await screen.findByRole("link", { name: "Northlight Films" })).toBeInTheDocument();
        await userEvent.click(screen.getByRole("button", { name: "Siguiente" }));
        await screen.findByText("Página 2 de 2");
        await userEvent.selectOptions(screen.getByLabelText("Filtrar por rol"), "Supplier");

        await waitFor(() => {
            expect(requests.filter((request) => request.path === "/api/contacts")).toHaveLength(3);
        });
        expect(screen.getByText("Northlight Films")).toBeInTheDocument();
    });

    it("creates a company with multiple roles and optional fields", async () => {
        const requests = stubApi({ "POST /api/contacts": () => json({ ...company, id: "saved-contact" }, 201) });
        renderAt("/contacts/new", [{ path: "/contacts/new", element: <ContactFormPage /> }]);

        await userEvent.selectOptions(await screen.findByLabelText("Tipo de contacto"), "Company");
        await userEvent.type(await screen.findByLabelText("Razón social"), "Northlight Films");
        await userEvent.type(await screen.findByLabelText(/Número de contacto/), "10022");
        await userEvent.click(screen.getByRole("checkbox", { name: "Cliente" }));
        await userEvent.click(screen.getByRole("checkbox", { name: "Proveedor" }));
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        await waitFor(() => {
            expect(requests).toHaveLength(1);
        });
        expect(requests[0]?.body).toMatchObject({
            type: "Company",
            legalName: "Northlight Films",
            contactNumber: "10022",
            roles: ["Customer", "Supplier"],
            taxId: null,
            email: null,
            address: null,
        });
    });

    it("preserves server validation errors when creating a private-person venue", async () => {
        stubApi({
            "POST /api/contacts": () => json({ code: "contacts.number-duplicate", detail: "Duplicated." }, 422),
        });
        renderAt("/contacts/new", [{ path: "/contacts/new", element: <ContactFormPage /> }]);

        await userEvent.selectOptions(await screen.findByLabelText("Tipo de contacto"), "PrivatePerson");
        await userEvent.type(await screen.findByLabelText("Razón social"), "Marina Costa");
        await userEvent.type(await screen.findByLabelText(/Número de contacto/), "10001");
        await userEvent.click(screen.getByRole("checkbox", { name: "Espacio" }));
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        expect(await screen.findByRole("alert")).toHaveTextContent("Duplicated.");
    });
});
