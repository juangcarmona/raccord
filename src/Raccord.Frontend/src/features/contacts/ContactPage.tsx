import { useEffect, useState, type SubmitEvent } from "react";
import { Link, useNavigate, useParams } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { AddressFields, readAddress } from "../../shared/ui/AddressFields";
import { Field, FormError, SelectField } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { formText } from "../../shared/ui/formText";

export function ContactPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { t } = useI18n();
    const [contact, setContact] = useState<Awaited<ReturnType<typeof loadContact>> | null>(null);
    const [error, setError] = useState<unknown>(null);
    useEffect(() => {
        if (id) void loadContact(id).then(setContact).catch(setError);
    }, [id]);
    if (!contact) return error ? <FormError error={error} /> : <p>{t("state.loading")}</p>;
    const contactId = contact.id;
    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        try {
            await unwrap(
                api.PUT("/api/contacts/{id}", {
                    params: { path: { id: contactId } },
                    body: {
                        legalName: formText(data, "legalName"),
                        type: Number(formText(data, "type")),
                        taxId: formText(data, "taxId") || null,
                        email: formText(data, "email") || null,
                        address: readAddress(data),
                    },
                }),
            );
            await navigate("/contacts");
        } catch (failure) {
            setError(failure);
        }
    }
    return (
        <>
            <PageHeader title={t("contacts.edit")} />
            <form className="panel" onSubmit={(event) => void submit(event)}>
                <div className="grid-form">
                    <Field label={t("common.legalName")} name="legalName" required defaultValue={contact.legalName} />
                    <SelectField label={t("contacts.type")} name="type" defaultValue={String(contact.type)}>
                        <option value="0">{t("contacts.customer")}</option>
                        <option value="1">{t("contacts.supplier")}</option>
                        <option value="2">{t("contacts.venue")}</option>
                    </SelectField>
                    <Field label={t("common.taxId")} name="taxId" defaultValue={contact.taxId ?? ""} />
                    <Field label={t("common.email")} name="email" defaultValue={contact.email ?? ""} type="email" />
                </div>
                <AddressFields legend={t("common.address")} required={false} initial={contact.address} />
                <div className="actions">
                    <button className="button button--primary" type="submit">
                        {t("common.save")}
                    </button>
                    <Link className="button button--quiet" to="/contacts">
                        {t("common.cancel")}
                    </Link>
                </div>
                <FormError error={error} />
            </form>
        </>
    );
}

async function loadContact(id: string) {
    return unwrap(api.GET("/api/contacts/{id}", { params: { path: { id } } }));
}
