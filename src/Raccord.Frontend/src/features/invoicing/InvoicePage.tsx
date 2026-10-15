import { useParams } from "react-router";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async } from "../../shared/ui/States";
import { LinesTable, Totals } from "../../shared/ui/Totals";
import { AuditTrail } from "./AuditTrail";

export function InvoicePage() {
    const { id = "" } = useParams();
    const query = useQuery(`invoice:${id}`, () => unwrap(api.GET("/api/invoices/{id}", { params: { path: { id } } })));
    return <Async query={query}>{(invoice) => <InvoiceView invoice={invoice} />}</Async>;
}

function Party({ title, party }: { title: string; party: Schemas["PartyResponse"] }) {
    const { t } = useI18n();
    const address = party.address;
    return (
        <section className="party" aria-label={title}>
            <h2>{title}</h2>
            <p className="party__name">{party.legalName}</p>
            <p>
                {t("common.taxId")}: {party.taxId}
            </p>
            {address && (
                <address>
                    {address.street}
                    <br />
                    {address.postalCode} {address.city}
                    {address.province && `, ${address.province}`} ({address.countryCode})
                </address>
            )}
        </section>
    );
}

function InvoiceView({ invoice }: { invoice: Schemas["IssuedInvoiceResponse"] }) {
    const { t, format, locale } = useI18n();
    return (
        <>
            <PageHeader
                title={invoice.number}
                actions={
                    <a
                        className="button button--primary"
                        href={`/api/invoices/${invoice.id}/document?language=${locale}`}
                        download
                    >
                        {t("invoices.download")}
                    </a>
                }
            >
                <p className="subtitle">
                    {t("invoices.issueDate")}: {format.date(invoice.issueDate)}
                    {invoice.operationDate && ` · ${t("drafts.operationDate")}: ${format.date(invoice.operationDate)}`}
                </p>
            </PageHeader>
            <p className="notice">{t("invoices.immutable")}</p>
            <div className="parties">
                <Party title={t("invoices.issuer")} party={invoice.issuer} />
                <Party title={t("invoices.recipient")} party={invoice.recipient} />
            </div>
            <LinesTable caption={t("projects.lines")} lines={invoice.lines} currency={invoice.totals.currency} />
            <Totals totals={invoice.totals} />
            <p className="fingerprint">
                {t("invoices.integrity")}: <code>{invoice.contentHash}</code>
            </p>
            <AuditTrail entityId={invoice.id} />
        </>
    );
}
