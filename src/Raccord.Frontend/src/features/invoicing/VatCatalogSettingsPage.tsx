import { useState, type SubmitEvent } from "react";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError, SelectField } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async } from "../../shared/ui/States";
import { formText } from "../../shared/ui/formText";

type Catalog = {
    classes: Schemas["VatClassResponse"][];
    schemes: Schemas["VatSchemeResponse"][];
};

async function loadCatalog(): Promise<Catalog> {
    const [classes, schemes] = await Promise.all([
        unwrap(api.GET("/api/invoicing/vat-classes")),
        unwrap(api.GET("/api/invoicing/vat-schemes")),
    ]);
    return { classes, schemes };
}

export function VatCatalogSettingsPage() {
    const { t } = useI18n();
    const query = useQuery("vat-catalog", loadCatalog);
    const [saved, setSaved] = useState(false);
    return (
        <>
            <PageHeader title={t("vat.title")}>
                <p className="subtitle">{t("vat.intro")}</p>
            </PageHeader>
            {saved && <p role="status">{t("vat.saved")}</p>}
            <Async query={query}>
                {(catalog) => (
                    <VatCatalogView
                        catalog={catalog}
                        onChanged={query.reload}
                        onSaveStarted={() => {
                            setSaved(false);
                        }}
                        onSaved={() => {
                            setSaved(true);
                        }}
                    />
                )}
            </Async>
        </>
    );
}

function VatCatalogView({
    catalog,
    onChanged,
    onSaveStarted,
    onSaved,
}: {
    catalog: Catalog;
    onChanged: () => void;
    onSaveStarted: () => void;
    onSaved: () => void;
}) {
    const { t } = useI18n();
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);
    const [newSchemeKind, setNewSchemeKind] = useState("per-class");

    async function mutate(operation: () => Promise<unknown>) {
        setBusy(true);
        onSaveStarted();
        setError(null);
        try {
            await operation();
            onSaved();
            onChanged();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function createClass(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        await mutate(async () => {
            await unwrap(
                api.POST("/api/invoicing/vat-classes", {
                    body: { name: formText(data, "className") },
                }),
            );
        });
    }

    async function createScheme(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        const kind = formText(data, "kind");
        await mutate(async () => {
            await unwrap(
                api.POST("/api/invoicing/vat-schemes", {
                    body: {
                        name: formText(data, "schemeName"),
                        kind,
                        fixedPercentage: kind === "fixed-rate" ? Number(formText(data, "percentage")) : null,
                        classRates:
                            kind === "per-class"
                                ? [
                                      {
                                          vatClassId: formText(data, "vatClassId"),
                                          percentage: Number(formText(data, "percentage")),
                                      },
                                  ]
                                : null,
                    },
                }),
            );
        });
    }

    return (
        <div className="stack">
            <p className="notice">{t("vat.disclaimer")}</p>
            <section className="panel" aria-labelledby="vat-classes-heading">
                <h2 id="vat-classes-heading">{t("vat.classes")}</h2>
                <ul>
                    {catalog.classes.map((vatClass) => (
                        <li key={vatClass.id}>{vatClass.name}</li>
                    ))}
                </ul>
                <form
                    className="grid-form"
                    onSubmit={(event) => {
                        void createClass(event);
                    }}
                >
                    <Field label={t("vat.newClass")} name="className" required maxLength={100} />
                    <div className="actions">
                        <button className="button" disabled={busy} type="submit">
                            {t("vat.addClass")}
                        </button>
                    </div>
                </form>
            </section>

            <section className="panel" aria-labelledby="vat-schemes-heading">
                <h2 id="vat-schemes-heading">{t("vat.schemes")}</h2>
                <ul className="stack">
                    {catalog.schemes.map((scheme) => (
                        <li key={scheme.id} className="panel">
                            <h3>{scheme.name}</h3>
                            <p>
                                {scheme.kind === "fixed-rate" && scheme.fixedPercentage !== null
                                    ? t("vat.rate", { rate: scheme.fixedPercentage })
                                    : scheme.kind === "per-class"
                                      ? t("vat.kind.perClass")
                                      : t("vat.kind.reverseCharge")}
                                {scheme.isDefault ? ` · ${t("vat.default")}` : ""}
                                {scheme.isUsed ? ` · ${t("vat.used")}` : ""}
                            </p>
                            {scheme.sourceUrl && scheme.verifiedOn && (
                                <p>
                                    {t("vat.source", { territory: scheme.territory ?? "", date: scheme.verifiedOn })}{" "}
                                    <a href={scheme.sourceUrl} target="_blank" rel="noreferrer">
                                        {t("vat.officialSource")}
                                    </a>
                                </p>
                            )}
                            {scheme.kind === "per-class" && (
                                <ul>
                                    {scheme.classRates.map((rate) => (
                                        <li key={rate.vatClassId}>
                                            {catalog.classes.find((item) => item.id === rate.vatClassId)?.name ??
                                                rate.vatClassId}
                                            : {rate.percentage}%
                                        </li>
                                    ))}
                                </ul>
                            )}
                            {!scheme.isDefault && (
                                <button
                                    className="button button--secondary"
                                    disabled={busy}
                                    onClick={() => {
                                        void mutate(async () => {
                                            await unwrap(
                                                api.PUT("/api/invoicing/vat-schemes/{id}/default", {
                                                    params: { path: { id: scheme.id } },
                                                }),
                                            );
                                        });
                                    }}
                                    type="button"
                                >
                                    {t("vat.makeDefault")}
                                </button>
                            )}
                            {scheme.kind === "fixed-rate" && !scheme.isUsed && (
                                <form
                                    className="grid-form"
                                    onSubmit={(event) => {
                                        event.preventDefault();
                                        const data = new FormData(event.currentTarget);
                                        void mutate(async () => {
                                            await unwrap(
                                                api.PUT("/api/invoicing/vat-schemes/{id}", {
                                                    params: { path: { id: scheme.id } },
                                                    body: {
                                                        name: scheme.name,
                                                        fixedPercentage: Number(formText(data, "updatedPercentage")),
                                                        classRates: null,
                                                    },
                                                }),
                                            );
                                        });
                                    }}
                                >
                                    <Field
                                        label={t("vat.editRate")}
                                        name="updatedPercentage"
                                        type="number"
                                        required
                                        min="0"
                                        max="100"
                                        step="0.01"
                                        defaultValue={String(scheme.fixedPercentage ?? 0)}
                                    />
                                    <div className="actions">
                                        <button className="button button--secondary" disabled={busy} type="submit">
                                            {t("common.save")}
                                        </button>
                                    </div>
                                </form>
                            )}
                            {scheme.kind === "per-class" && !scheme.isUsed && (
                                <form
                                    className="grid-form"
                                    onSubmit={(event) => {
                                        event.preventDefault();
                                        const data = new FormData(event.currentTarget);
                                        void mutate(async () => {
                                            await unwrap(
                                                api.PUT("/api/invoicing/vat-schemes/{id}", {
                                                    params: { path: { id: scheme.id } },
                                                    body: {
                                                        name: scheme.name,
                                                        fixedPercentage: null,
                                                        classRates: scheme.classRates.map((rate) => ({
                                                            vatClassId: rate.vatClassId,
                                                            percentage: Number(
                                                                formText(data, `rate-${rate.vatClassId}`),
                                                            ),
                                                        })),
                                                    },
                                                }),
                                            );
                                        });
                                    }}
                                >
                                    {scheme.classRates.map((rate) => {
                                        const mappedClass = catalog.classes.find((item) => item.id === rate.vatClassId);
                                        return (
                                            <Field
                                                key={rate.vatClassId}
                                                label={`${mappedClass?.name ?? rate.vatClassId} · ${t("vat.editRate")}`}
                                                name={`rate-${rate.vatClassId}`}
                                                type="number"
                                                required
                                                min="0"
                                                max="100"
                                                step="0.01"
                                                defaultValue={String(rate.percentage)}
                                            />
                                        );
                                    })}
                                    <div className="actions">
                                        <button className="button button--secondary" disabled={busy} type="submit">
                                            {t("common.save")}
                                        </button>
                                    </div>
                                </form>
                            )}
                            {scheme.isUsed && <p>{t("vat.rateFrozen")}</p>}
                        </li>
                    ))}
                </ul>
                <form
                    className="grid-form"
                    onSubmit={(event) => {
                        void createScheme(event);
                    }}
                >
                    <Field label={t("vat.newScheme")} name="schemeName" required maxLength={100} />
                    <SelectField
                        label={t("vat.schemeKind")}
                        name="kind"
                        required
                        value={newSchemeKind}
                        onChange={(event) => {
                            setNewSchemeKind(event.currentTarget.value);
                        }}
                    >
                        <option value="per-class">{t("vat.kind.perClass")}</option>
                        <option value="fixed-rate">{t("vat.kind.fixedRate")}</option>
                        <option value="reverse-charge">{t("vat.kind.reverseCharge")}</option>
                    </SelectField>
                    {newSchemeKind === "per-class" && (
                        <SelectField label={t("vat.classForScheme")} name="vatClassId" required>
                            {catalog.classes.map((vatClass) => (
                                <option key={vatClass.id} value={vatClass.id}>
                                    {vatClass.name}
                                </option>
                            ))}
                        </SelectField>
                    )}
                    {newSchemeKind !== "reverse-charge" && (
                        <Field
                            label={t("vat.percentage")}
                            name="percentage"
                            type="number"
                            required
                            min="0"
                            max="100"
                            step="0.01"
                        />
                    )}
                    <div className="actions">
                        <button
                            className="button"
                            disabled={busy || (newSchemeKind === "per-class" && catalog.classes.length === 0)}
                            type="submit"
                        >
                            {t("vat.addScheme")}
                        </button>
                    </div>
                </form>
            </section>
            <FormError error={error} />
        </div>
    );
}
