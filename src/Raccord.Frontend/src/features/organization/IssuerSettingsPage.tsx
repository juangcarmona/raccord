import { useState, type SubmitEvent } from "react";
import { api, ApiError, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { AddressFields, readAddress } from "../../shared/ui/AddressFields";
import { Field, FormError } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async } from "../../shared/ui/States";
import { formText } from "../../shared/ui/formText";

async function loadIssuer(): Promise<Schemas["IssuerProfileResponse"] | null> {
    try {
        return await unwrap(api.GET("/api/organization/issuer"));
    } catch (error) {
        if (error instanceof ApiError && error.isNotFound) return null;
        throw error;
    }
}

export function IssuerSettingsPage() {
    const { t } = useI18n();
    const query = useQuery("issuer", loadIssuer);
    return (
        <>
            <PageHeader title={t("settings.title")}>
                <p className="subtitle">{t("settings.intro")}</p>
            </PageHeader>
            <Async query={query}>{(issuer) => <IssuerForm issuer={issuer} />}</Async>
        </>
    );
}

function IssuerForm({ issuer }: { issuer: Schemas["IssuerProfileResponse"] | null }) {
    const { t } = useI18n();
    const [error, setError] = useState<unknown>(null);
    const [saved, setSaved] = useState(false);
    const [busy, setBusy] = useState(false);

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const address = readAddress(data);
        setBusy(true);
        setError(null);
        setSaved(false);
        try {
            await unwrap(
                api.PUT("/api/organization/issuer", {
                    body: {
                        legalName: formText(data, "legalName"),
                        taxId: formText(data, "taxId"),
                        address: address ?? { street: "", postalCode: "", city: "", province: "", countryCode: "" },
                    },
                }),
            );
            setSaved(true);
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    return (
        <form
            className="panel"
            onSubmit={(event) => {
                void submit(event);
            }}
        >
            {!issuer && <p className="notice notice--warning">{t("settings.missing")}</p>}
            <div className="grid-form">
                <Field
                    label={t("common.legalName")}
                    name="legalName"
                    required
                    maxLength={200}
                    defaultValue={issuer?.legalName ?? ""}
                    autoComplete="organization"
                />
                <Field
                    label={t("common.taxId")}
                    name="taxId"
                    required
                    maxLength={32}
                    defaultValue={issuer?.taxId ?? ""}
                />
            </div>
            <AddressFields legend={t("common.address")} required initial={issuer?.address ?? null} />
            <div className="actions">
                <button type="submit" className="button button--primary" disabled={busy}>
                    {t("common.save")}
                </button>
            </div>
            <p role="status" aria-live="polite">
                {saved ? t("settings.saved") : ""}
            </p>
            <FormError error={error} />
        </form>
    );
}
