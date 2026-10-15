import { useState, type SubmitEvent } from "react";
import { Link, useNavigate, useParams } from "react-router";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { FormError } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async } from "../../shared/ui/States";
import { LinesTable, Totals } from "../../shared/ui/Totals";

function localToday(): string {
    const now = new Date();
    const pad = (value: number) => String(value).padStart(2, "0");
    return `${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDate())}`;
}

export function DraftPage() {
    const { id = "" } = useParams();
    const query = useQuery(`draft:${id}`, () =>
        unwrap(api.GET("/api/invoice-drafts/{id}", { params: { path: { id } } })),
    );
    return <Async query={query}>{(draft) => <DraftView draft={draft} onChanged={query.reload} />}</Async>;
}

function DraftView({ draft, onChanged }: { draft: Schemas["InvoiceDraftResponse"]; onChanged: () => void }) {
    const { t, format, describe } = useI18n();
    const navigate = useNavigate();
    const [issueDate, setIssueDate] = useState(localToday);
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState<unknown>(null);
    const open = draft.status === "open";

    async function run(action: () => Promise<void>) {
        setBusy(true);
        setError(null);
        try {
            await action();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    function finalize(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        void run(async () => {
            const invoice = await unwrap(
                api.POST("/api/invoice-drafts/{id}/finalize", {
                    params: { path: { id: draft.id } },
                    body: { issueDate },
                }),
            );
            await navigate(`/invoices/${invoice.id}`);
        });
    }

    function removeLine(lineId: string) {
        void run(async () => {
            await unwrap(
                api.DELETE("/api/invoice-drafts/{id}/lines/{lineId}", { params: { path: { id: draft.id, lineId } } }),
            );
            onChanged();
        });
    }

    function discard() {
        void run(async () => {
            await unwrap(api.DELETE("/api/invoice-drafts/{id}", { params: { path: { id: draft.id } } }));
            await navigate(draft.projectId ? `/projects/${draft.projectId}` : "/drafts");
        });
    }

    return (
        <>
            <PageHeader
                title={t("drafts.heading", {
                    project: draft.projectName ?? t("drafts.standalone"),
                })}
            >
                <p className="subtitle">
                    {draft.customerName} · {t("drafts.series")} {draft.seriesCode} · {t("drafts.operationDate")}{" "}
                    {format.date(draft.operationDate)}
                </p>
            </PageHeader>

            <LinesTable
                caption={t("projects.lines")}
                lines={draft.lines}
                currency={draft.totals.currency}
                {...(open && draft.lines.length > 1
                    ? {
                          renderAction: (line: Schemas["InvoiceLineResponse"]) =>
                              line.id && (
                                  <button
                                      type="button"
                                      className="button button--quiet"
                                      disabled={busy}
                                      onClick={() => {
                                          removeLine(line.id ?? "");
                                      }}
                                  >
                                      {t("drafts.removeLine", { description: line.description })}
                                  </button>
                              ),
                      }
                    : {})}
            />
            <Totals totals={draft.totals} />

            {open ? (
                <section className="panel" aria-labelledby="finalize-heading">
                    <h2 id="finalize-heading">{t("drafts.finalize")}</h2>
                    {draft.validationIssues.length > 0 ? (
                        <div className="notice notice--warning">
                            <p>{t("drafts.issues")}</p>
                            <ul>
                                {draft.validationIssues.map((issue) => (
                                    <li key={issue.code}>{describe("issue", issue.code, issue.message)}</li>
                                ))}
                            </ul>
                        </div>
                    ) : (
                        <p className="notice">{t("drafts.ready")}</p>
                    )}
                    <form onSubmit={finalize} className="inline-form">
                        <div className="field">
                            <label htmlFor="issue-date">{t("drafts.issueDate")}</label>
                            <input
                                id="issue-date"
                                type="date"
                                required
                                value={issueDate}
                                onChange={(event) => {
                                    setIssueDate(event.target.value);
                                }}
                            />
                        </div>
                        <button
                            type="submit"
                            className="button button--primary"
                            disabled={busy || draft.validationIssues.length > 0}
                            aria-describedby="finalize-hint"
                        >
                            {t("drafts.finalize")}
                        </button>
                        <button type="button" className="button button--danger" disabled={busy} onClick={discard}>
                            {t("drafts.discard")}
                        </button>
                    </form>
                    <p id="finalize-hint" className="field__hint">
                        {t("drafts.finalizeHint")}
                    </p>
                    <FormError error={error} />
                </section>
            ) : (
                <p className="notice">
                    {t("drafts.issued")}{" "}
                    {draft.issuedInvoiceId && (
                        <Link to={`/invoices/${draft.issuedInvoiceId}`}>{t("drafts.viewInvoice")}</Link>
                    )}
                </p>
            )}
        </>
    );
}
