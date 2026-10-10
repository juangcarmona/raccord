import { useState, type SubmitEvent } from "react";
import { Link, useNavigate } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError, SelectField } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async, Empty } from "../../shared/ui/States";
import { formText } from "../../shared/ui/formText";

export function NewProjectPage() {
    const { t } = useI18n();
    const navigate = useNavigate();
    const customers = useQuery("customers", () => unwrap(api.GET("/api/customers")));
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        setBusy(true);
        setError(null);
        try {
            const project = await unwrap(
                api.POST("/api/projects", {
                    body: {
                        name: formText(data, "name"),
                        customerId: formText(data, "customerId"),
                        startDate: formText(data, "startDate"),
                        endDate: formText(data, "endDate"),
                    },
                }),
            );
            await navigate(`/projects/${project.id}`);
        } catch (failure) {
            setError(failure);
            setBusy(false);
        }
    }

    return (
        <>
            <PageHeader title={t("projects.new")} />
            <Async query={customers}>
                {(list) =>
                    list.length === 0 ? (
                        <Empty>
                            {t("projects.noCustomers")} <Link to="/customers/new">{t("customers.new")}</Link>
                        </Empty>
                    ) : (
                        <form
                            className="grid-form panel"
                            onSubmit={(event) => {
                                void submit(event);
                            }}
                        >
                            <Field label={t("projects.name")} name="name" required maxLength={200} />
                            <SelectField label={t("common.customer")} name="customerId" required>
                                {list.map((customer) => (
                                    <option key={customer.id} value={customer.id}>
                                        {customer.legalName}
                                    </option>
                                ))}
                            </SelectField>
                            <Field label={t("projects.startDate")} name="startDate" type="date" required />
                            <Field label={t("projects.endDate")} name="endDate" type="date" required />
                            <div className="actions">
                                <button type="submit" className="button button--primary" disabled={busy}>
                                    {t("common.save")}
                                </button>
                                <Link className="button button--quiet" to="/projects">
                                    {t("common.cancel")}
                                </Link>
                            </div>
                            <FormError error={error} />
                        </form>
                    )
                }
            </Async>
        </>
    );
}
