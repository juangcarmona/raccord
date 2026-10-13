import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { CustomersPage } from "./CustomersPage";
import { NewCustomerPage } from "./NewCustomerPage";
import { json, renderAt, stubApi } from "../../test/render";

describe("customer routes", () => {
    it("shows the empty state and new-customer action", async () => {
        stubApi({ "GET /api/customers": () => json([]) });
        renderAt("/customers", [{ path: "/customers", element: <CustomersPage /> }]);

        expect(await screen.findByText("Todavía no hay clientes.")).toBeInTheDocument();
        expect(screen.getByRole("link", { name: "Nuevo cliente" })).toHaveAttribute("href", "/customers/new");
    });

    it("submits a new customer with optional fields normalized to null", async () => {
        const requests = stubApi({ "POST /api/customers": () => json({ id: "customer-1" }, 201) });
        renderAt("/customers/new", [{ path: "/customers/new", element: <NewCustomerPage /> }]);

        await userEvent.type(screen.getByLabelText("Razón social"), "Northlight Films");
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        await waitFor(() => {
            expect(requests).toHaveLength(1);
        });
        expect(requests[0]?.body).toMatchObject({
            legalName: "Northlight Films",
            taxId: null,
            email: null,
            address: null,
        });
    });
});
