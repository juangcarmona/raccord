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
    const [folderId, setFolderId] = useState("");
    const [tagIds, setTagIds] = useState<string[]>([]);
    const [tagMatch, setTagMatch] = useState("any");
    const tagsKey = tagIds.toSorted().join(",");
    const query = useQuery(`contacts:${page}:${role}:${folderId}:${tagsKey}:${tagMatch}`, async () => {
        const [result, tags, folders] = await Promise.all([
            unwrap(
                api.GET("/api/contacts", {
                    params: {
                        query: {
                            page,
                            pageSize,
                            ...(role ? { role } : {}),
                            ...(folderId ? { folderId } : {}),
                            ...(tagIds.length ? { tagIds, tagMatch } : {}),
                        },
                    },
                }),
            ),
            unwrap(api.GET("/api/contacts/tags")),
            unwrap(api.GET("/api/contacts/folders")),
        ]);
        return { result, tags, folders };
    });

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
            <Async query={query}>
                {({ result, tags, folders }) => (
                    <>
                        <div className="panel grid-form">
                            <label htmlFor="contact-role-filter">
                                <span>{t("contacts.filterRole")}</span>
                                <select
                                    id="contact-role-filter"
                                    value={role}
                                    onChange={(event) => {
                                        setRole(event.currentTarget.value);
                                        setPage(1);
                                    }}
                                >
                                    <option value="">{t("contacts.allRoles")}</option>
                                    <option value="Customer">{t("contacts.role.customer")}</option>
                                    <option value="Supplier">{t("contacts.role.supplier")}</option>
                                    <option value="Venue">{t("contacts.role.venue")}</option>
                                </select>
                            </label>
                            <label>
                                <span>{t("contacts.organize.filterFolder")}</span>
                                <select
                                    value={folderId}
                                    onChange={(event) => {
                                        setFolderId(event.currentTarget.value);
                                        setPage(1);
                                    }}
                                >
                                    <option value="">{t("contacts.organize.allFolders")}</option>
                                    {folders.map((folder) => (
                                        <option key={folder.id} value={folder.id}>
                                            {folder.name}
                                        </option>
                                    ))}
                                </select>
                            </label>
                            <fieldset>
                                <legend>{t("contacts.organize.filterTags")}</legend>
                                {tags.map((tag) => (
                                    <label key={tag.id}>
                                        <input
                                            type="checkbox"
                                            checked={tagIds.includes(tag.id)}
                                            onChange={(event) => {
                                                const checked = event.currentTarget.checked;
                                                setTagIds((current) =>
                                                    checked
                                                        ? [...current, tag.id]
                                                        : current.filter((id) => id !== tag.id),
                                                );
                                                setPage(1);
                                            }}
                                        />
                                        {tag.name}
                                    </label>
                                ))}
                                {tags.length === 0 && (
                                    <p className="state state--empty">{t("contacts.organize.noTags")}</p>
                                )}
                            </fieldset>
                            <label>
                                <span>{t("contacts.organize.tagMatch")}</span>
                                <select
                                    value={tagMatch}
                                    disabled={tagIds.length < 2}
                                    onChange={(event) => {
                                        setTagMatch(event.currentTarget.value);
                                        setPage(1);
                                    }}
                                >
                                    <option value="any">{t("contacts.organize.anyTag")}</option>
                                    <option value="all">{t("contacts.organize.allTags")}</option>
                                </select>
                            </label>
                        </div>
                        {result.items.length === 0 ? (
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
                                                <th scope="col">{t("common.actions")}</th>
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
                                                    <td>
                                                        {contact.roles.map((item) => roleLabel(t, item)).join(", ")}
                                                    </td>
                                                    <td>{contact.address?.city ?? "—"}</td>
                                                    <td>
                                                        <Link to={`/contacts/${contact.id}/organization`}>
                                                            {t("contacts.organize.action")}
                                                        </Link>
                                                    </td>
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
                        )}
                    </>
                )}
            </Async>
        </>
    );
}

function roleLabel(t: ReturnType<typeof useI18n>["t"], role: string) {
    if (role === "Customer") return t("contacts.role.customer");
    if (role === "Supplier") return t("contacts.role.supplier");
    return t("contacts.role.venue");
}
