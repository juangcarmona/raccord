import { Link } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async, Empty } from "../../shared/ui/States";

export function ContactsPage() {
    const { t } = useI18n();
    const query = useQuery("contacts", () =>
        unwrap(api.GET("/api/contacts", { params: { query: { page: 1, pageSize: 20 } } })),
    );
    return (
        <>
            <PageHeader
                title={t("contacts.title")}
                actions={
                    <Link className="button button--primary" to="/contacts/new">
                        {t("contacts.new")}
                    </Link>
                }
            />
            <Async query={query}>
                {(page) =>
                    page.items.length === 0 ? (
                        <Empty>{t("contacts.empty")}</Empty>
                    ) : (
                        <div className="table-scroll">
                            <table>
                                <caption className="visually-hidden">{t("contacts.title")}</caption>
                                <thead>
                                    <tr>
                                        <th>{t("common.legalName")}</th>
                                        <th>{t("contacts.type")}</th>
                                        <th>{t("common.taxId")}</th>
                                        <th />
                                    </tr>
                                </thead>
                                <tbody>
                                    {page.items.map((contact) => (
                                        <tr key={contact.id}>
                                            <th scope="row">
                                                <Link to={`/contacts/${contact.id}`}>{contact.legalName}</Link>
                                            </th>
                                            <td>
                                                {
                                                    [
                                                        t("contacts.customer"),
                                                        t("contacts.supplier"),
                                                        t("contacts.venue"),
                                                    ][contact.type]
                                                }
                                            </td>
                                            <td>{contact.taxId ?? "—"}</td>
                                            <td>
                                                {contact.isInvoiceable
                                                    ? t("customers.invoiceable")
                                                    : t("customers.incomplete")}
                                            </td>
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
