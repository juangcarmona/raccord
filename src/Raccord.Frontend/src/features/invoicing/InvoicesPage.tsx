import { Link } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async, Empty } from "../../shared/ui/States";

export function InvoicesPage() {
    const { t, format } = useI18n();
    const query = useQuery("invoices", () => unwrap(api.GET("/api/invoices")));
    return (
        <>
            <PageHeader title={t("invoices.title")} />
            <Async query={query}>
                {(invoices) =>
                    invoices.length === 0 ? (
                        <Empty>{t("invoices.empty")}</Empty>
                    ) : (
                        <div className="table-scroll">
                            <table>
                                <caption className="visually-hidden">{t("invoices.title")}</caption>
                                <thead>
                                    <tr>
                                        <th scope="col">{t("invoices.number")}</th>
                                        <th scope="col">{t("invoices.issueDate")}</th>
                                        <th scope="col">{t("invoices.recipient")}</th>
                                        <th scope="col" className="num">
                                            {t("common.total")}
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {invoices.map((invoice) => (
                                        <tr key={invoice.id}>
                                            <th scope="row">
                                                <Link to={`/invoices/${invoice.id}`}>{invoice.number}</Link>
                                            </th>
                                            <td>{format.date(invoice.issueDate)}</td>
                                            <td>{invoice.recipientName}</td>
                                            <td className="num">{format.money(invoice.total, invoice.currency)}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )
                }
            </Async>
        </>
    );
}
