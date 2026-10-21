import { useState, type SubmitEvent } from "react";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError, SelectField } from "../../shared/ui/Form";
import { Async } from "../../shared/ui/States";
import { formText } from "../../shared/ui/formText";

const kinds = ["equipment", "labour", "transport", "consumable", "service", "other"] as const;

async function loadVatCatalog() {
    const [classes, schemes] = await Promise.all([
        unwrap(api.GET("/api/invoicing/vat-classes")),
        unwrap(api.GET("/api/invoicing/vat-schemes")),
    ]);
    return { classes, schemes };
}

export function AddBillableLineForm({ projectId, onAdded }: { projectId: string; onAdded: () => void }) {
    const { t } = useI18n();
    const catalog = useQuery("project-vat-catalog", loadVatCatalog);
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);
    const [selectedVatClassId, setSelectedVatClassId] = useState("");
    const [selectedVatSchemeId, setSelectedVatSchemeId] = useState("");

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const form = event.currentTarget;
        const data = new FormData(form);
        const number = (name: string) => Number(formText(data, name));
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.POST("/api/projects/{id}/billable-lines", {
                    params: { path: { id: projectId } },
                    body: {
                        kind: formText(data, "kind"),
                        description: formText(data, "description"),
                        quantity: number("quantity"),
                        unitPrice: number("unitPrice"),
                        discountPercent: number("discountPercent"),
                        vatClassId: formText(data, "vatClassId"),
                        vatSchemeId: formText(data, "vatSchemeId"),
                    },
                }),
            );
            form.reset();
            onAdded();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    return (
        <Async query={catalog}>
            {(vatCatalog) =>
                (() => {
                    const defaultScheme = vatCatalog.schemes.find((scheme) => scheme.isDefault);
                    const defaultClassId = defaultScheme?.classRates[0]?.vatClassId ?? vatCatalog.classes[0]?.id ?? "";
                    const effectiveClassId = selectedVatClassId || defaultClassId;
                    const applicableSchemes = vatCatalog.schemes.filter(
                        (scheme) =>
                            scheme.kind !== "per-class" ||
                            scheme.classRates.some((rate) => rate.vatClassId === effectiveClassId),
                    );
                    const defaultApplicableSchemeId =
                        applicableSchemes.find((scheme) => scheme.isDefault)?.id ?? applicableSchemes[0]?.id ?? "";
                    const effectiveSchemeId = applicableSchemes.some((scheme) => scheme.id === selectedVatSchemeId)
                        ? selectedVatSchemeId
                        : defaultApplicableSchemeId;
                    return (
                        <section className="panel" aria-labelledby="add-line-heading">
                            <h2 id="add-line-heading">{t("projects.addLine")}</h2>
                            <form
                                onSubmit={(event) => {
                                    void submit(event);
                                }}
                                className="grid-form"
                            >
                                <SelectField label={t("projects.kind")} name="kind" defaultValue="equipment">
                                    {kinds.map((kind) => (
                                        <option key={kind} value={kind}>
                                            {t(`kind.${kind}`)}
                                        </option>
                                    ))}
                                </SelectField>
                                <Field label={t("common.description")} name="description" required maxLength={500} />
                                <Field
                                    label={t("common.quantity")}
                                    name="quantity"
                                    type="number"
                                    required
                                    min="0.0001"
                                    step="0.0001"
                                    defaultValue="1"
                                />
                                <Field
                                    label={t("common.unitPrice")}
                                    name="unitPrice"
                                    type="number"
                                    required
                                    min="0"
                                    step="0.0001"
                                />
                                <Field
                                    label={`${t("common.discount")} (%)`}
                                    name="discountPercent"
                                    type="number"
                                    required
                                    min="0"
                                    max="100"
                                    step="0.01"
                                    defaultValue="0"
                                />
                                <SelectField
                                    label={t("vat.class")}
                                    name="vatClassId"
                                    required
                                    value={effectiveClassId}
                                    onChange={(event) => {
                                        setSelectedVatClassId(event.currentTarget.value);
                                        setSelectedVatSchemeId("");
                                    }}
                                >
                                    {vatCatalog.classes.map((vatClass) => (
                                        <option key={vatClass.id} value={vatClass.id}>
                                            {vatClass.name}
                                        </option>
                                    ))}
                                </SelectField>
                                <SelectField
                                    label={t("vat.scheme")}
                                    name="vatSchemeId"
                                    required
                                    value={effectiveSchemeId}
                                    onChange={(event) => {
                                        setSelectedVatSchemeId(event.currentTarget.value);
                                    }}
                                >
                                    {applicableSchemes.map((scheme) => (
                                        <option key={scheme.id} value={scheme.id}>
                                            {scheme.name}
                                        </option>
                                    ))}
                                </SelectField>
                                <div className="actions">
                                    <button
                                        type="submit"
                                        className="button"
                                        disabled={busy || !effectiveClassId || !effectiveSchemeId}
                                    >
                                        {t("projects.addLine")}
                                    </button>
                                </div>
                            </form>
                            <FormError error={error} />
                        </section>
                    );
                })()
            }
        </Async>
    );
}
