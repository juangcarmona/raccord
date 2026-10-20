import { useState, type ChangeEvent } from "react";
import { Link } from "react-router";
import { uploadContactsImport, type Schemas } from "../../shared/api/client";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { FormError } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";

const fields = [
    { key: "type", label: "contacts.import.field.type", required: true },
    { key: "legalName", label: "contacts.import.field.legalName", required: true },
    { key: "roles", label: "contacts.import.field.roles", required: true },
    { key: "contactNumber", label: "contacts.import.field.number", required: false },
    { key: "taxId", label: "common.taxId", required: false },
    { key: "email", label: "common.email", required: false },
    { key: "street", label: "common.street", required: false },
    { key: "postalCode", label: "common.postalCode", required: false },
    { key: "city", label: "common.city", required: false },
    { key: "province", label: "common.province", required: false },
    { key: "countryCode", label: "common.country", required: false },
] as const;

type Mapping = Record<string, number>;

export function ContactImportPage() {
    const { t } = useI18n();
    const [file, setFile] = useState<File | null>(null);
    const [hasHeader, setHasHeader] = useState(true);
    const [inspection, setInspection] = useState<Schemas["ContactImportInspectionResponse"] | null>(null);
    const [mapping, setMapping] = useState<Mapping>({});
    const [preview, setPreview] = useState<Schemas["ContactImportPreviewResponse"] | null>(null);
    const [result, setResult] = useState<Schemas["ContactImportResultResponse"] | null>(null);
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);

    async function inspect(selected: File | null, withHeader: boolean) {
        setFile(selected);
        setInspection(null);
        setPreview(null);
        setResult(null);
        setError(null);
        setMapping({});
        if (!selected) return;

        const form = new FormData();
        form.append("file", selected);
        form.append("hasHeader", String(withHeader));
        setBusy(true);
        try {
            const columns = await uploadContactsImport<Schemas["ContactImportInspectionResponse"]>(
                "/api/contacts/import/inspect",
                form,
            );
            setInspection(columns);
            setMapping(defaultMapping(columns.headers));
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    function changeHeader(event: ChangeEvent<HTMLInputElement>) {
        const withHeader = event.currentTarget.checked;
        setHasHeader(withHeader);
        if (file) void inspect(file, withHeader);
    }

    async function submitPreview() {
        if (!file || !inspection) return;
        const form = createForm(file, { hasHeader, fields: mapping });
        setBusy(true);
        setError(null);
        setPreview(null);
        setResult(null);
        try {
            setPreview(
                await uploadContactsImport<Schemas["ContactImportPreviewResponse"]>(
                    "/api/contacts/import/preview",
                    form,
                ),
            );
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function confirmImport() {
        if (!file || !preview) return;
        const form = createForm(file, { hasHeader, fields: mapping });
        form.append("sha256", preview.sha256);
        setBusy(true);
        setError(null);
        try {
            setResult(await uploadContactsImport<Schemas["ContactImportResultResponse"]>("/api/contacts/import", form));
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    return (
        <>
            <PageHeader title={t("contacts.import.title")} />
            <section className="panel" aria-labelledby="contacts-import-upload-heading">
                <h2 id="contacts-import-upload-heading">{t("contacts.import.chooseFile")}</h2>
                <label>
                    <span>{t("contacts.import.file")}</span>
                    <input
                        aria-label={t("contacts.import.file")}
                        type="file"
                        accept=".csv,.xlsx"
                        onChange={(event) => void inspect(event.currentTarget.files?.[0] ?? null, hasHeader)}
                    />
                </label>
                <label className="checkbox-label">
                    <input type="checkbox" checked={hasHeader} onChange={changeHeader} />
                    {t("contacts.import.headerRow")}
                </label>
                <p>{t("contacts.import.fileLimits")}</p>
                {inspection && (
                    <>
                        <h3>{t("contacts.import.columns")}</h3>
                        <div className="grid-form">
                            {fields.map((field) => (
                                <label key={field.key}>
                                    <span>
                                        {t(field.label)} {field.required ? "*" : ""}
                                    </span>
                                    <select
                                        aria-label={t(field.label)}
                                        value={mapping[field.key] ?? ""}
                                        required={field.required}
                                        onChange={(event) => {
                                            setMapping((current) => ({
                                                ...current,
                                                [field.key]: Number(event.currentTarget.value),
                                            }));
                                        }}
                                    >
                                        <option value="">{t("contacts.import.ignoreField")}</option>
                                        {inspection.headers.map((header, index) => (
                                            <option key={`${index}:${header}`} value={index}>
                                                {header || `${t("contacts.import.column")} ${index + 1}`}
                                            </option>
                                        ))}
                                    </select>
                                </label>
                            ))}
                        </div>
                        <div className="table-scroll">
                            <table>
                                <caption>{t("contacts.import.sampleRows")}</caption>
                                <thead>
                                    <tr>
                                        {inspection.headers.map((header, index) => (
                                            <th key={`${index}:${header}`} scope="col">
                                                {header || `${t("contacts.import.column")} ${index + 1}`}
                                            </th>
                                        ))}
                                    </tr>
                                </thead>
                                <tbody>
                                    {inspection.sampleRows.map((row, rowIndex) => (
                                        <tr key={rowIndex}>
                                            {inspection.headers.map((_, column) => (
                                                <td key={column}>{row[column] ?? ""}</td>
                                            ))}
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                        <div className="actions">
                            <button
                                className="button button--primary"
                                type="button"
                                disabled={
                                    busy || fields.some((field) => field.required && mapping[field.key] === undefined)
                                }
                                onClick={() => void submitPreview()}
                            >
                                {t("contacts.import.preview")}
                            </button>
                        </div>
                    </>
                )}
            </section>

            {preview && (
                <section className="panel" aria-labelledby="contacts-import-preview-heading">
                    <h2 id="contacts-import-preview-heading">{t("contacts.import.previewTitle")}</h2>
                    <p>
                        {t("contacts.import.counts", {
                            total: preview.totalRowCount,
                            valid: preview.validRowCount,
                            rejected: preview.rejectedRowCount,
                        })}
                    </p>
                    <div className="table-scroll">
                        <table>
                            <caption>{t("contacts.import.previewRows")}</caption>
                            <thead>
                                <tr>
                                    <th scope="col">{t("contacts.import.rowNumber")}</th>
                                    {Object.keys(preview.rows[0]?.values ?? {}).map((key) => (
                                        <th scope="col" key={key}>
                                            {t(fieldLabel(key))}
                                        </th>
                                    ))}
                                    <th scope="col">{t("contacts.import.validation")}</th>
                                </tr>
                            </thead>
                            <tbody>
                                {preview.rows.map((row) => (
                                    <tr key={row.rowNumber}>
                                        <th scope="row">{row.rowNumber}</th>
                                        {Object.keys(row.values).map((key) => (
                                            <td key={key}>{row.values[key]}</td>
                                        ))}
                                        <td>
                                            {row.errors.length === 0
                                                ? t("contacts.import.valid")
                                                : row.errors.join("; ")}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                    <div className="actions">
                        <button
                            className="button button--primary"
                            type="button"
                            disabled={busy}
                            onClick={() => void confirmImport()}
                        >
                            {t("contacts.import.confirm", { count: preview.validRowCount })}
                        </button>
                    </div>
                </section>
            )}

            {result && (
                <section className="panel" aria-live="polite">
                    <h2>{t("contacts.import.complete")}</h2>
                    <p>
                        {t("contacts.import.counts", {
                            total: result.acceptedRowCount + result.rejectedRowCount,
                            valid: result.acceptedRowCount,
                            rejected: result.rejectedRowCount,
                        })}
                    </p>
                    {result.rejectedRows.length > 0 && (
                        <ul aria-label={t("contacts.import.rejectedRows")}>
                            {result.rejectedRows.map((row) => (
                                <li key={row.rowNumber}>
                                    {t("contacts.import.rejected", {
                                        row: row.rowNumber,
                                        reason: row.errors.join("; "),
                                    })}
                                </li>
                            ))}
                        </ul>
                    )}
                    <Link className="button button--quiet" to="/contacts">
                        {t("contacts.title")}
                    </Link>
                </section>
            )}
            <FormError error={error} />
        </>
    );
}

function createForm(file: File, mapping: { hasHeader: boolean; fields: Mapping }) {
    const form = new FormData();
    form.append("file", file);
    form.append("hasHeader", String(mapping.hasHeader));
    form.append("mapping", JSON.stringify(mapping));
    return form;
}

function defaultMapping(headers: readonly string[]): Mapping {
    const normalized = headers.map((header) => header.trim().toLocaleLowerCase());
    const find = (...names: string[]) => normalized.findIndex((header) => names.includes(header));
    const entries: [string, number][] = [
        ["type", find("type", "tipo")],
        ["legalName", find("legalname", "legal name", "name", "nombre", "razón social", "razon social")],
        ["roles", find("roles", "role", "rol", "roles de contacto")],
        ["contactNumber", find("contactnumber", "contact number", "número de contacto", "numero de contacto")],
        ["taxId", find("taxid", "tax id", "cif", "nif", "identificación fiscal")],
        ["email", find("email", "correo", "correo electrónico")],
    ];
    return Object.fromEntries(entries.filter(([, index]) => index >= 0));
}

function fieldLabel(key: string): (typeof fields)[number]["label"] {
    return fields.find((field) => field.key === key)?.label ?? "contacts.import.field.type";
}
