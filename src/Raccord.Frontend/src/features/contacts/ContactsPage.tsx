import { useState } from "react";
import { Link } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async, Empty } from "../../shared/ui/States";

const pageSize = 10;

export function ContactsPage() {
    const { t } = useI18n();
    const [page, setPage] = useState(1);
    const [role, setRole] = useState("");
    const query = useQuery(`contacts:${page}:${role}`, () =>
        unwrap(
            api.GET("/api/contacts", {
                params: { query: { page, pageSize, ...(role ? { role } : {}) } },
            }),
        ),
    );

    function changeRole(value: string) {
        setRole(value);
        setPage(1);
    }

    return (
        <>
            <PageHeader
                title={t("contacts.title")}
                actions={
                    <div className="actions">
                        <Link className="button button--quiet" to="/contacts/import">
                            {t("contacts.import.action")}
                        </Link>
                        <Link className="button button--primary" to="/contacts/new">
                            {t("contacts.new")}
                        </Link>
                    </div>
                }
            />
            <div className="panel">
                <label htmlFor="contact-role-filter">{t("contacts.filterRole")}</label>
                <select
                    id="contact-role-filter"
                    value={role}
                    onChange={(event) => {
                        changeRole(event.currentTarget.value);
                    }}
                >
                    <option value="">{t("contacts.allRoles")}</option>
                    <option value="Customer">{t("contacts.role.customer")}</option>
                    <option value="Supplier">{t("contacts.role.supplier")}</option>
                    <option value="Venue">{t("contacts.role.venue")}</option>
                </select>
            </div>
            <Async query={query}>
                {(result) =>
                    result.items.length === 0 ? (
                        <Empty>{t("contacts.empty")}</Empty>
                    ) : (
                        <>
                            <div className="table-scroll">
                                <table>
                                    <caption className="visually-hidden">{t("contacts.title")}</caption>
                                    <thead>
                                        <tr>
                                            <th scope="col">{t("contacts.number")}</th>
                                            <th scope="col">{t("contacts.type")}</th>
                                            <th scope="col">{t("common.legalName")}</th>
                                            <th scope="col">{t("contacts.roles")}</th>
                                            <th scope="col">{t("common.city")}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {result.items.map((contact) => (
                                            <tr key={contact.id}>
                                                <td>{contact.contactNumber}</td>
                                                <td>
                                                    {contact.type === "Company"
                                                        ? t("contacts.type.company")
                                                        : t("contacts.type.person")}
                                                </td>
                                                <th scope="row">
                                                    <Link to={`/contacts/${contact.id}`}>{contact.legalName}</Link>
                                                </th>
                                                <td>{contact.roles.map((item) => roleLabel(t, item)).join(", ")}</td>
                                                <td>{contact.address?.city ?? "—"}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                            <nav className="actions" aria-label={t("contacts.pagination")}>
                                <button
                                    className="button button--quiet"
                                    type="button"
                                    disabled={!result.hasPreviousPage}
                                    onClick={() => {
                                        setPage((current) => Math.max(1, current - 1));
                                    }}
                                >
                                    {t("contacts.previous")}
                                </button>
                                <span>
                                    {t("contacts.page", {
                                        page: result.page,
                                        pages: Math.max(1, Math.ceil(result.totalCount / result.pageSize)),
                                    })}
                                </span>
                                <button
                                    className="button button--quiet"
                                    type="button"
                                    disabled={!result.hasNextPage}
                                    onClick={() => {
                                        setPage((current) => current + 1);
                                    }}
                                >
                                    {t("contacts.next")}
                                </button>
                            </nav>
                        </>
                    )
                }
            </Async>
        </>
    );
}

function roleLabel(t: ReturnType<typeof useI18n>["t"], role: string) {
    if (role === "Customer") return t("contacts.role.customer");
    if (role === "Supplier") return t("contacts.role.supplier");
    return t("contacts.role.venue");
}
