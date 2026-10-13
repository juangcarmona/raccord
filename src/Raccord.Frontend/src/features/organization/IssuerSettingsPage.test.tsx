import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { IssuerSettingsPage } from "./IssuerSettingsPage";
import { json, renderAt, stubApi } from "../../test/render";

describe("IssuerSettingsPage", () => {
    it("shows missing-profile guidance and saves the issuer", async () => {
        const requests = stubApi({
            "GET /api/organization/issuer": () => json({}, 404),
            "PUT /api/organization/issuer": () => json({ legalName: "Northlight Films" }),
        });
        renderAt("/settings/issuer", [{ path: "/settings/issuer", element: <IssuerSettingsPage /> }]);

        expect(await screen.findByText("Configura la empresa emisora antes de emitir facturas.")).toBeInTheDocument();
        await userEvent.type(screen.getByLabelText("Razón social"), "Northlight Films");
        await userEvent.type(screen.getByLabelText("NIF"), "ESA12345678");
        await userEvent.type(screen.getByLabelText("Calle y número"), "Calle Mayor 1");
        await userEvent.type(screen.getByLabelText("Código postal"), "28001");
        await userEvent.type(screen.getByLabelText("Localidad"), "Madrid");
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        await waitFor(() => {
            expect(requests.some((request) => request.method === "PUT")).toBe(true);
        });
        expect(await screen.findByText("Datos guardados.")).toBeInTheDocument();
    });
});
