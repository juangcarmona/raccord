import { useState, type SubmitEvent } from "react";
import { api, unwrap } from "../../shared/api/client";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError, SelectField } from "../../shared/ui/Form";
import { formText } from "../../shared/ui/formText";

const kinds = ["equipment", "labour", "transport", "consumable", "service", "other"] as const;

export function AddBillableLineForm({ projectId, onAdded }: { projectId: string; onAdded: () => void }) {
    const { t } = useI18n();
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);

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
                        vatRate: number("vatRate"),
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
                <Field label={t("common.unitPrice")} name="unitPrice" type="number" required min="0" step="0.0001" />
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
                <Field
                    label={`${t("common.vat")} (%)`}
                    name="vatRate"
                    type="number"
                    required
                    min="0"
                    max="100"
                    step="0.01"
                    defaultValue="21"
                />
                <div className="actions">
                    <button type="submit" className="button" disabled={busy}>
                        {t("projects.addLine")}
                    </button>
                </div>
            </form>
            <FormError error={error} />
        </section>
    );
}
