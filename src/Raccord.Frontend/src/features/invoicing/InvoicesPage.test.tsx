import { screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { InvoicesPage } from "./InvoicesPage";
import { json, renderAt, stubApi } from "../../test/render";

describe("InvoicesPage", () => {
    it("renders an issued invoice row and detail link", async () => {
        stubApi({
            "GET /api/invoices": () =>
                json([
                    {
                        id: "invoice-1",
                        number: "INV-2026-0001",
                        issueDate: "2026-01-15",
                        recipientName: "Northlight Films",
                        total: 1250,
                        currency: "EUR",
                    },
                ]),
        });
        renderAt("/invoices", [{ path: "/invoices", element: <InvoicesPage /> }]);

        expect(await screen.findByRole("link", { name: "INV-2026-0001" })).toHaveAttribute(
            "href",
            "/invoices/invoice-1",
        );
        expect(screen.getByText("Northlight Films")).toBeInTheDocument();
    });

    it("shows the empty state when there are no issued invoices", async () => {
        stubApi({ "GET /api/invoices": () => json([]) });
        renderAt("/invoices", [{ path: "/invoices", element: <InvoicesPage /> }]);

        expect(await screen.findByText("Todavía no se ha emitido ninguna factura.")).toBeInTheDocument();
    });
});
