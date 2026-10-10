import { useState, type SubmitEvent } from "react";
import { Link, useNavigate } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { AddressFields, readAddress } from "../../shared/ui/AddressFields";
import { Field, FormError } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { formText } from "../../shared/ui/formText";

export function NewCustomerPage() {
    const { t } = useI18n();
    const navigate = useNavigate();
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const optional = (name: string) => {
            const value = formText(data, name).trim();
            return value === "" ? null : value;
        };
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.POST("/api/customers", {
                    body: {
                        legalName: formText(data, "legalName"),
                        taxId: optional("taxId"),
                        email: optional("email"),
                        address: readAddress(data),
                    },
                }),
            );
            await navigate("/customers");
        } catch (failure) {
            setError(failure);
            setBusy(false);
        }
    }

    return (
        <>
            <PageHeader title={t("customers.new")} />
            <form
                className="panel"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <div className="grid-form">
                    <Field
                        label={t("common.legalName")}
                        name="legalName"
                        required
                        maxLength={200}
                        autoComplete="organization"
                    />
                    <Field label={t("common.taxId")} name="taxId" maxLength={32} />
                    <Field label={t("common.email")} name="email" type="email" maxLength={320} autoComplete="email" />
                </div>
                <AddressFields legend={t("common.address")} required={false} />
                <div className="actions">
                    <button type="submit" className="button button--primary" disabled={busy}>
                        {t("common.save")}
                    </button>
                    <Link className="button button--quiet" to="/customers">
                        {t("common.cancel")}
                    </Link>
                </div>
                <FormError error={error} />
            </form>
        </>
    );
}
