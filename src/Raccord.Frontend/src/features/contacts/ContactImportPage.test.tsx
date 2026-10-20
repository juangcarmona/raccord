import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { json, renderAt, stubApi } from "../../test/render";
import { ContactImportPage } from "./ContactImportPage";

describe("contact import", () => {
    it("maps columns, reviews validation, imports valid rows and shows rejected rows", async () => {
        const requests = stubApi({
            "POST /api/contacts/import/inspect": () =>
                json({
                    fileType: "csv",
                    headers: ["Type", "Legal Name", "Roles"],
                    sampleRows: [["Company", "Northlight Films", "Customer"]],
                }),
            "POST /api/contacts/import/preview": () =>
                json({
                    sha256: "preview-digest",
                    fileType: "csv",
                    totalRowCount: 2,
                    validRowCount: 1,
                    rejectedRowCount: 1,
                    headers: ["Type", "Legal Name", "Roles"],
                    rows: [
                        {
                            rowNumber: 2,
                            values: { type: "Company", legalName: "Northlight Films", roles: "Customer" },
                            errors: [],
                        },
                        {
                            rowNumber: 3,
                            values: { type: "Company", legalName: "", roles: "Supplier" },
                            errors: ["Legal name is required."],
                        },
                    ],
                }),
            "POST /api/contacts/import": () =>
                json({
                    importId: "import-1",
                    acceptedRowCount: 1,
                    rejectedRowCount: 1,
                    rejectedRows: [{ rowNumber: 3, values: { legalName: "" }, errors: ["Legal name is required."] }],
                }),
        });
        renderAt("/contacts/import", [{ path: "/contacts/import", element: <ContactImportPage /> }]);
        const file = new File(
            ["type,legal name,roles\ncompany,Northlight Films,customer\ncompany,,supplier"],
            "contacts.csv",
            { type: "text/csv" },
        );
        await userEvent.upload(screen.getByLabelText("Archivo CSV o XLSX"), file);

        expect(await screen.findByRole("table", { name: "Muestra de filas" })).toBeInTheDocument();
        expect(screen.getByLabelText("Tipo de contacto")).toHaveValue("0");
        await userEvent.click(screen.getByRole("button", { name: "Validar y previsualizar" }));

        expect(await screen.findByText("Filas: 2. Válidas: 1. Rechazadas: 1.")).toBeInTheDocument();
        expect(screen.getByText("Legal name is required.")).toBeInTheDocument();
        await userEvent.click(screen.getByRole("button", { name: "Importar 1 contactos válidos" }));

        expect(await screen.findByText("Importación completada")).toBeInTheDocument();
        expect(screen.getByText("Fila 3: Legal name is required.")).toBeInTheDocument();
        await waitFor(() => {
            expect(requests.map((request) => request.path)).toEqual([
                "/api/contacts/import/inspect",
                "/api/contacts/import/preview",
                "/api/contacts/import",
            ]);
        });
        const importForm = requests[2]?.body as FormData;
        expect(importForm.get("sha256")).toBe("preview-digest");
        const mapping = importForm.get("mapping");
        expect(typeof mapping).toBe("string");
        expect(JSON.parse(mapping as string) as unknown).toMatchObject({
            hasHeader: true,
            fields: { type: 0, legalName: 1, roles: 2 },
        });
    });

    it("shows file inspection failures", async () => {
        stubApi({
            "POST /api/contacts/import/inspect": () => json({ detail: "Upload failed." }, 400),
        });
        renderAt("/contacts/import", [{ path: "/contacts/import", element: <ContactImportPage /> }]);
        await userEvent.upload(
            screen.getByLabelText("Archivo CSV o XLSX"),
            new File(["type,name,roles\n"], "contacts.csv", { type: "text/csv" }),
        );

        expect(await screen.findByRole("alert")).toHaveTextContent("Upload failed.");
    });
});
