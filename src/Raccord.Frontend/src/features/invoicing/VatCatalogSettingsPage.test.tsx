import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { json, renderAt, stubApi } from "../../test/render";
import { VatCatalogSettingsPage } from "./VatCatalogSettingsPage";

describe("VatCatalogSettingsPage", () => {
    it("shows the dated initial rate and changes the default for new lines", async () => {
        let defaultId = "scheme-standard";
        const requests = stubApi({
            "GET /api/invoicing/vat-classes": () => json([{ id: "class-general", name: "General" }]),
            "GET /api/invoicing/vat-schemes": () =>
                json([
                    {
                        id: "scheme-standard",
                        name: "General",
                        kind: "per-class",
                        fixedPercentage: null,
                        isDefault: defaultId === "scheme-standard",
                        isUsed: false,
                        territory: "ES",
                        sourceUrl: "https://sede.agenciatributaria.gob.es/example",
                        verifiedOn: "2026-10-21",
                        classRates: [{ vatClassId: "class-general", percentage: 21 }],
                    },
                    {
                        id: "scheme-zero",
                        name: "Zero rate",
                        kind: "fixed-rate",
                        fixedPercentage: 0,
                        isDefault: defaultId === "scheme-zero",
                        isUsed: false,
                        territory: null,
                        sourceUrl: null,
                        verifiedOn: null,
                        classRates: [],
                    },
                ]),
            "PUT /api/invoicing/vat-schemes/scheme-zero/default": () => {
                defaultId = "scheme-zero";
                return json({ id: defaultId });
            },
        });
        renderAt("/settings/vat", [{ path: "/settings/vat", element: <VatCatalogSettingsPage /> }]);

        expect(await screen.findByText("Referencia inicial de ES, comprobada el 2026-10-21:")).toBeInTheDocument();
        expect(screen.getByText("General: 21%", { exact: true })).toBeInTheDocument();
        await userEvent.click(screen.getByRole("button", { name: "Usar como predeterminado" }));

        await waitFor(() => {
            expect(requests.some((request) => request.path.endsWith("/scheme-zero/default"))).toBe(true);
        });
        expect(await screen.findByText("Cambios guardados.")).toBeInTheDocument();
    });

    it("keeps an invalid class name in the form when the server rejects it", async () => {
        stubApi({
            "GET /api/invoicing/vat-classes": () => json([{ id: "class-general", name: "General" }]),
            "GET /api/invoicing/vat-schemes": () => json([]),
            "POST /api/invoicing/vat-classes": () =>
                json({ code: "invoicing.vat-class-name-invalid", detail: "Name is required." }, 422),
        });
        renderAt("/settings/vat", [{ path: "/settings/vat", element: <VatCatalogSettingsPage /> }]);

        const name = await screen.findByLabelText("Nombre de la clase");
        await userEvent.type(name, "Reduced");
        await userEvent.click(screen.getByRole("button", { name: "Añadir clase" }));

        expect(await screen.findByRole("alert")).toBeInTheDocument();
        expect(name).toHaveValue("Reduced");
    });

    it("creates a rate scheme mapped only to the selected VAT class", async () => {
        const requests = stubApi({
            "GET /api/invoicing/vat-classes": () =>
                json([
                    { id: "class-general", name: "General" },
                    { id: "class-exempt", name: "Exempt" },
                ]),
            "GET /api/invoicing/vat-schemes": () => json([]),
            "POST /api/invoicing/vat-schemes": () => json({ id: "scheme-custom" }, 201),
        });
        renderAt("/settings/vat", [{ path: "/settings/vat", element: <VatCatalogSettingsPage /> }]);

        await userEvent.type(await screen.findByLabelText("Nombre del esquema"), "Reduced");
        await userEvent.selectOptions(screen.getByLabelText("Clase para el esquema"), "class-exempt");
        await userEvent.type(screen.getByLabelText("Tipo (%)"), "10");
        await userEvent.click(screen.getByRole("button", { name: "Añadir esquema" }));

        await waitFor(() => {
            const created = requests.find((request) => request.method === "POST");
            expect(created?.body).toEqual({
                name: "Reduced",
                kind: "per-class",
                fixedPercentage: null,
                classRates: [{ vatClassId: "class-exempt", percentage: 10 }],
            });
        });
    });

    it("edits an unused per-class rate while retaining its class identity", async () => {
        const requests = stubApi({
            "GET /api/invoicing/vat-classes": () => json([{ id: "class-exempt", name: "Exempt" }]),
            "GET /api/invoicing/vat-schemes": () =>
                json([
                    {
                        id: "scheme-reduced",
                        name: "Reduced",
                        kind: "per-class",
                        fixedPercentage: null,
                        isDefault: false,
                        isUsed: false,
                        territory: null,
                        sourceUrl: null,
                        verifiedOn: null,
                        classRates: [{ vatClassId: "class-exempt", percentage: 10 }],
                    },
                ]),
            "PUT /api/invoicing/vat-schemes/scheme-reduced": () => json({ id: "scheme-reduced" }),
        });
        renderAt("/settings/vat", [{ path: "/settings/vat", element: <VatCatalogSettingsPage /> }]);

        await userEvent.clear(await screen.findByLabelText("Exempt · Nuevo tipo (%)"));
        await userEvent.type(screen.getByLabelText("Exempt · Nuevo tipo (%)"), "12");
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        await waitFor(() => {
            const update = requests.find((request) => request.method === "PUT");
            expect(update?.body).toEqual({
                name: "Reduced",
                fixedPercentage: null,
                classRates: [{ vatClassId: "class-exempt", percentage: 12 }],
            });
        });
    });

    it("creates a reverse-charge scheme without borrowing a standard-rate default", async () => {
        const requests = stubApi({
            "GET /api/invoicing/vat-classes": () => json([{ id: "class-general", name: "General" }]),
            "GET /api/invoicing/vat-schemes": () => json([]),
            "POST /api/invoicing/vat-schemes": () => json({ id: "scheme-reverse" }, 201),
        });
        renderAt("/settings/vat", [{ path: "/settings/vat", element: <VatCatalogSettingsPage /> }]);

        await userEvent.type(await screen.findByLabelText("Nombre del esquema"), "Reverse charge");
        await userEvent.selectOptions(screen.getByLabelText("Tipo de esquema"), "reverse-charge");
        await userEvent.click(screen.getByRole("button", { name: "Añadir esquema" }));

        await waitFor(() => {
            const created = requests.find((request) => request.method === "POST");
            expect(created?.body).toEqual({
                name: "Reverse charge",
                kind: "reverse-charge",
                fixedPercentage: null,
                classRates: null,
            });
        });
    });

    it("preserves an unmapped scheme form when the server rejects its class mapping", async () => {
        stubApi({
            "GET /api/invoicing/vat-classes": () => json([{ id: "class-general", name: "General" }]),
            "GET /api/invoicing/vat-schemes": () => json([]),
            "POST /api/invoicing/vat-schemes": () =>
                json(
                    {
                        code: "invoicing.vat-class-unmapped",
                        detail: "The selected class is unavailable.",
                    },
                    422,
                ),
        });
        renderAt("/settings/vat", [{ path: "/settings/vat", element: <VatCatalogSettingsPage /> }]);

        await userEvent.type(await screen.findByLabelText("Nombre del esquema"), "Reduced");
        await userEvent.type(screen.getByLabelText("Tipo (%)"), "10");
        await userEvent.click(screen.getByRole("button", { name: "Añadir esquema" }));

        expect(await screen.findByRole("alert")).toHaveTextContent("The selected class is unavailable.");
        expect(screen.getByLabelText("Nombre del esquema")).toHaveValue("Reduced");
        expect(screen.getByLabelText("Tipo (%)")).toHaveValue(10);
    });
});
