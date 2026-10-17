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
        await userEvent.type(screen.getByLabelText("País (ISO, p. ej. ES)"), "ES");
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        await waitFor(() => {
            expect(requests.some((request) => request.method === "PUT")).toBe(true);
        });
        expect(await screen.findByText("Datos guardados.")).toBeInTheDocument();
    });

    it("submits post-issuance legal identity changes for second-person review", async () => {
        const requests = stubApi({
            "GET /api/organization/issuer": () =>
                json({
                    legalName: "Northlight Films",
                    taxId: "B00000000",
                    hasIssuedInvoice: true,
                    address: {
                        street: "Calle Mayor 1",
                        postalCode: "28001",
                        city: "Madrid",
                        province: "Madrid",
                        countryCode: "ES",
                    },
                }),
            "GET /api/organization/issuer-identity-changes": () => json([]),
            "PUT /api/organization/issuer": () => json({ legalName: "Northlight Films", taxId: "B00000000" }),
            "POST /api/organization/issuer-identity-changes": () => json({ id: "request-1", status: "pending" }, 201),
        });
        renderAt("/settings/issuer", [{ path: "/settings/issuer", element: <IssuerSettingsPage /> }]);

        await userEvent.clear(await screen.findByLabelText("Razón social"));
        await userEvent.type(screen.getByLabelText("Razón social"), "Northlight Media S.L.");
        await userEvent.clear(screen.getByLabelText("NIF"));
        await userEvent.type(screen.getByLabelText("NIF"), "B12345678");
        await userEvent.type(screen.getByLabelText("Motivo del cambio de identidad legal"), "Actualización registral");
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        expect(
            await screen.findByText(
                "Cambio enviado para revisión. La identidad actual sigue activa hasta su aprobación.",
            ),
        ).toBeInTheDocument();
        const create = requests.find((request) => request.method === "POST");
        expect(create?.body).toMatchObject({
            legalName: "Northlight Media S.L.",
            taxId: "B12345678",
            reason: "Actualización registral",
        });
    });

    it("lets a distinct administrator approve a pending identity request", async () => {
        let approved = false;
        stubApi({
            "GET /api/organization/issuer": () =>
                json({
                    legalName: "Northlight Films",
                    taxId: "B00000000",
                    hasIssuedInvoice: true,
                    address: {
                        street: "Calle Mayor 1",
                        postalCode: "28001",
                        city: "Madrid",
                        province: "Madrid",
                        countryCode: "ES",
                    },
                }),
            "GET /api/organization/issuer-identity-changes": () =>
                json([
                    {
                        id: "request-1",
                        oldLegalName: "Northlight Films",
                        oldTaxId: "B00000000",
                        proposedLegalName: "Northlight Media S.L.",
                        proposedTaxId: "B12345678",
                        reason: "Actualización registral",
                        requestedById: "requester",
                        requestedByName: "Requester",
                        requestedAt: "2026-10-17T12:00:00Z",
                        status: approved ? "approved" : "pending",
                        isRequester: false,
                    },
                ]),
            "PUT /api/organization/issuer-identity-changes/request-1": () => {
                approved = true;
                return json({ status: "approved" });
            },
        });
        renderAt("/settings/issuer", [{ path: "/settings/issuer", element: <IssuerSettingsPage /> }]);

        await userEvent.click(await screen.findByRole("button", { name: "Aprobar cambio" }));
        expect(await screen.findByText("Estado: Aprobada")).toBeInTheDocument();
    });
});
