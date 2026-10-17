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

async function loadIdentityChanges(): Promise<Schemas["IssuerIdentityChangeResponse"][]> {
    return await unwrap(api.GET("/api/organization/issuer-identity-changes"));
}

export function IssuerSettingsPage() {
    const { t } = useI18n();
    const query = useQuery("issuer", loadIssuer);
    const changes = useQuery("issuer-identity-changes", loadIdentityChanges);
    const reloadAll = () => {
        query.reload();
        changes.reload();
    };
    return (
        <>
            <PageHeader title={t("settings.title")}>
                <p className="subtitle">{t("settings.intro")}</p>
            </PageHeader>
            <Async query={query}>
                {(issuer) => (
                    <IssuerForm
                        issuer={issuer}
                        changes={changes}
                        reloadChanges={changes.reload}
                        reloadAll={reloadAll}
                    />
                )}
            </Async>
        </>
    );
}

function IssuerForm({
    issuer,
    changes,
    reloadChanges,
    reloadAll,
}: {
    issuer: Schemas["IssuerProfileResponse"] | null;
    changes: ReturnType<typeof useQuery<Schemas["IssuerIdentityChangeResponse"][]>>;
    reloadChanges: () => void;
    reloadAll: () => void;
}) {
    const { t } = useI18n();
    const [error, setError] = useState<unknown>(null);
    const [saved, setSaved] = useState<"saved" | "pending" | false>(false);
    const [busy, setBusy] = useState(false);

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const address = readAddress(data);
        setBusy(true);
        setError(null);
        setSaved(false);
        try {
            const legalName = formText(data, "legalName");
            const taxId = formText(data, "taxId");
            const profileAddress = address ?? { street: "", postalCode: "", city: "", province: "", countryCode: "" };
            const identityChanged =
                issuer?.hasIssuedInvoice === true && (legalName !== issuer.legalName || taxId !== issuer.taxId);
            if (identityChanged) {
                await unwrap(
                    api.PUT("/api/organization/issuer", {
                        body: { legalName: issuer.legalName, taxId: issuer.taxId, address: profileAddress },
                    }),
                );
                await unwrap(
                    api.POST("/api/organization/issuer-identity-changes", {
                        body: { legalName, taxId, reason: formText(data, "reason") },
                    }),
                );
            } else {
                await unwrap(
                    api.PUT("/api/organization/issuer", {
                        body: { legalName, taxId, address: profileAddress },
                    }),
                );
            }
            setSaved(identityChanged ? "pending" : "saved");
            reloadChanges();
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
            {issuer?.hasIssuedInvoice && (
                <Field label={t("settings.changeReason")} name="reason" required maxLength={2000} />
            )}
            <div className="actions">
                <button type="submit" className="button button--primary" disabled={busy}>
                    {t("common.save")}
                </button>
            </div>
            <p role="status" aria-live="polite">
                {saved ? t(saved === "pending" ? "settings.requested" : "settings.saved") : ""}
            </p>
            <FormError error={error} />
            <IdentityChangeHistory changes={changes} onChanged={reloadAll} />
        </form>
    );
}

function IdentityChangeHistory({
    changes,
    onChanged,
}: {
    changes: ReturnType<typeof useQuery<Schemas["IssuerIdentityChangeResponse"][]>>;
    onChanged: () => void;
}) {
    const { t } = useI18n();
    const [error, setError] = useState<unknown>(null);
    const [busyId, setBusyId] = useState<string | null>(null);
    if (changes.status !== "success" || changes.data.length === 0) return null;

    async function decide(id: string, approve: boolean) {
        setBusyId(id);
        setError(null);
        try {
            await unwrap(
                api.PUT("/api/organization/issuer-identity-changes/{id}", {
                    params: { path: { id } },
                    body: { approve },
                }),
            );
            onChanged();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusyId(null);
        }
    }

    return (
        <section aria-labelledby="identity-change-title">
            <h2 id="identity-change-title">{t("settings.identityChanges")}</h2>
            {changes.data.map((change) => (
                <article className="panel" key={change.id}>
                    <p>
                        {t("settings.identityChangeStatus", {
                            status: t(
                                `settings.status.${change.status}` as
                                    | "settings.status.pending"
                                    | "settings.status.approved"
                                    | "settings.status.rejected",
                            ),
                        })}
                    </p>
                    <p>{t("settings.identityChangeOld", { name: change.oldLegalName, taxId: change.oldTaxId })}</p>
                    <p>
                        {t("settings.identityChangeProposed", {
                            name: change.proposedLegalName,
                            taxId: change.proposedTaxId,
                        })}
                    </p>
                    <p>{t("settings.identityChangeRequester", { name: change.requestedByName })}</p>
                    <p>{t("settings.identityChangeReason", { reason: change.reason })}</p>
                    {change.status === "pending" && !change.isRequester && (
                        <div className="actions">
                            <button
                                type="button"
                                disabled={busyId === change.id}
                                onClick={() => void decide(change.id, true)}
                            >
                                {t("settings.approveIdentityChange")}
                            </button>
                            <button
                                type="button"
                                disabled={busyId === change.id}
                                onClick={() => void decide(change.id, false)}
                            >
                                {t("settings.rejectIdentityChange")}
                            </button>
                        </div>
                    )}
                </article>
            ))}
            <FormError error={error} />
        </section>
    );
}
