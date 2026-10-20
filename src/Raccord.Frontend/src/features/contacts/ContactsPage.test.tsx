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
            expect(requests.some((request) => request.method === "PUT")).toBe(true);
        });
        expect(requests.find((request) => request.method === "PUT")?.body).toMatchObject({
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

describe("contact people", () => {
    it("shows an empty state when the contact has no people", async () => {
        stubApi({
            "GET /api/contacts/contact-1": () => json(company),
            "GET /api/contacts/contact-1/people": () => json([]),
        });
        renderAt("/contacts/contact-1", [{ path: "/contacts/:id", element: <ContactFormPage /> }]);

        expect(await screen.findByText("Todavía no hay personas asociadas a este contacto.")).toBeInTheDocument();
        expect(screen.getByRole("heading", { name: "Añadir persona" })).toBeInTheDocument();
    });

    it("shows a loading state before the directory responds", async () => {
        let resolvePeople: ((response: Response) => void) | undefined;
        stubApi({
            "GET /api/contacts/contact-1": () => json(company),
            "GET /api/contacts/contact-1/people": () =>
                new Promise<Response>((resolve) => {
                    resolvePeople = resolve;
                }),
        });
        renderAt("/contacts/contact-1", [{ path: "/contacts/:id", element: <ContactFormPage /> }]);

        expect(await screen.findByRole("status")).toHaveTextContent("Cargando");
        resolvePeople?.(json([]));
        expect(await screen.findByRole("heading", { name: "Personas de contacto" })).toBeInTheDocument();
    });

    it("creates, lists and edits contact people with the default and administrative designations", async () => {
        const person = {
            id: "person-1",
            contactId: "contact-1",
            name: "Marina Costa",
            email: "marina@example.test",
            phone: "+34 600 123 456",
            isDefault: true,
            isAdministrative: true,
        };
        const requests = stubApi({
            "GET /api/contacts/contact-1": () => json(company),
            "GET /api/contacts/contact-1/people": () => json([person]),
            "POST /api/contacts/contact-1/people": () => json(person, 201),
            "PUT /api/contacts/contact-1/people/person-1": () =>
                json({ ...person, name: "Marina Costa Editada", isAdministrative: false }),
        });
        renderAt("/contacts/contact-1", [{ path: "/contacts/:id", element: <ContactFormPage /> }]);

        expect(await screen.findByText("Marina Costa")).toBeInTheDocument();
        expect(screen.getByText("Persona predeterminada")).toBeInTheDocument();
        expect(screen.getByText("Persona administrativa")).toBeInTheDocument();
        await userEvent.type(screen.getByLabelText("Nombre de la persona"), "Nueva Persona");
        await userEvent.type(screen.getByLabelText(/Correo de la persona/), "nueva@example.test");
        await userEvent.type(screen.getByLabelText(/Teléfono de la persona/), "+34 600 000 000");
        await userEvent.click(screen.getByLabelText("Persona predeterminada"));
        await userEvent.click(screen.getByLabelText("Persona administrativa"));
        await userEvent.click(screen.getByRole("button", { name: "Añadir persona" }));

        await waitFor(() => {
            expect(requests.some((request) => request.method === "POST")).toBe(true);
        });
        expect(requests.find((request) => request.method === "POST")?.body).toMatchObject({
            name: "Nueva Persona",
            email: "nueva@example.test",
            phone: "+34 600 000 000",
            isDefault: true,
            isAdministrative: true,
        });
        expect(screen.getByLabelText("Nombre de la persona")).toHaveValue("");
        await userEvent.click(screen.getByRole("button", { name: "Editar persona" }));
        await userEvent.clear(screen.getByLabelText("Nombre de la persona"));
        await userEvent.type(screen.getByLabelText("Nombre de la persona"), "Marina Costa Editada");
        await userEvent.click(screen.getByLabelText("Persona administrativa"));
        await userEvent.click(screen.getByRole("button", { name: "Guardar cambios" }));
        await waitFor(() => {
            expect(requests.some((request) => request.method === "PUT")).toBe(true);
        });
        expect(requests.find((request) => request.method === "PUT")?.body).toMatchObject({
            name: "Marina Costa Editada",
            isAdministrative: false,
        });
    });

    it("shows the people request error and retries it", async () => {
        let calls = 0;
        stubApi({
            "GET /api/contacts/contact-1": () => json(company),
            "GET /api/contacts/contact-1/people": () => {
                calls += 1;
                return calls === 1 ? json({ title: "Unavailable" }, 503) : json([]);
            },
        });
        renderAt("/contacts/contact-1", [{ path: "/contacts/:id", element: <ContactFormPage /> }]);

        expect(await screen.findByRole("alert")).toBeInTheDocument();
        await userEvent.click(screen.getByRole("button", { name: "Reintentar" }));
        expect(await screen.findByRole("heading", { name: "Personas de contacto" })).toBeInTheDocument();
    });

    it("shows a server validation message when saving a person fails", async () => {
        stubApi({
            "GET /api/contacts/contact-1": () => json(company),
            "GET /api/contacts/contact-1/people": () => json([]),
            "POST /api/contacts/contact-1/people": () => json({ detail: "La persona administrativa ya existe." }, 409),
        });
        renderAt("/contacts/contact-1", [{ path: "/contacts/:id", element: <ContactFormPage /> }]);

        await userEvent.type(await screen.findByLabelText("Nombre de la persona"), "Marina Costa");
        await userEvent.click(screen.getByRole("button", { name: "Añadir persona" }));
        expect(await screen.findByRole("alert")).toHaveTextContent("La persona administrativa ya existe.");
    });
});
