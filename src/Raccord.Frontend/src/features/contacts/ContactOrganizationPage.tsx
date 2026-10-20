import { useState, type SubmitEvent } from "react";
import { Link, useParams } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async } from "../../shared/ui/States";

export function ContactOrganizationPage() {
    const { id = "" } = useParams();
    const query = useQuery(`contact-organization:${id}`, async () => {
        const [contact, organization, tags, folders] = await Promise.all([
            unwrap(api.GET("/api/contacts/{id}", { params: { path: { id } } })),
            unwrap(api.GET("/api/contacts/{id}/organization", { params: { path: { id } } })),
            unwrap(api.GET("/api/contacts/tags")),
            unwrap(api.GET("/api/contacts/folders")),
        ]);
        return { contact, organization, tags, folders };
    });
    return (
        <Async query={query}>
            {(data) => (
                <OrganizationForm
                    key={id}
                    id={id}
                    name={data.contact.legalName}
                    state={data.organization}
                    tags={data.tags}
                    folders={data.folders}
                    reload={query.reload}
                />
            )}
        </Async>
    );
}

type Folder = { id: string; name: string; parentId: string | null; position: number };

function OrganizationForm({
    id,
    name,
    state,
    tags,
    folders,
    reload,
}: {
    id: string;
    name: string;
    state: { folderId: string | null; tags: { id: string; name: string }[] };
    tags: { id: string; name: string }[];
    folders: Folder[];
    reload: () => void;
}) {
    const { t } = useI18n();
    const [tagName, setTagName] = useState("");
    const [folderName, setFolderName] = useState("");
    const [parentId, setParentId] = useState("");
    const [error, setError] = useState<unknown>(null);

    async function perform(action: () => Promise<unknown>) {
        setError(null);
        try {
            await action();
            reload();
        } catch (failure) {
            setError(failure);
        }
    }

    async function createTag(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        await perform(async () => {
            await unwrap(api.POST("/api/contacts/tags", { body: { name: tagName.trim() } }));
            setTagName("");
        });
    }

    async function createFolder(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        await perform(async () => {
            await unwrap(
                api.POST("/api/contacts/folders", {
                    body: { name: folderName.trim(), parentId: parentId || null },
                }),
            );
            setFolderName("");
        });
    }

    async function updateFolder(folder: Folder, changes: Partial<Folder>) {
        await perform(() =>
            unwrap(
                api.PUT("/api/contacts/folders/{folderId}", {
                    params: { path: { folderId: folder.id } },
                    body: {
                        name: changes.name ?? folder.name,
                        parentId: changes.parentId === undefined ? folder.parentId : changes.parentId,
                        position: changes.position ?? folder.position,
                    },
                }),
            ),
        );
    }

    return (
        <>
            <PageHeader title={t("contacts.organize.title", { name })} />
            <section className="panel" aria-labelledby="contact-tags-heading">
                <h2 id="contact-tags-heading">{t("contacts.organize.tags")}</h2>
                <form className="actions" onSubmit={(event) => void createTag(event)}>
                    <Field
                        label={t("contacts.organize.newTag")}
                        value={tagName}
                        onChange={(event) => {
                            setTagName(event.currentTarget.value);
                        }}
                        required
                    />
                    <button className="button button--primary" type="submit">
                        {t("contacts.organize.createTag")}
                    </button>
                </form>
                <fieldset>
                    <legend>{t("contacts.organize.assignedTags")}</legend>
                    <ul>
                        {tags.map((tag) => (
                            <li key={tag.id}>
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={state.tags.some((item) => item.id === tag.id)}
                                        onChange={(event) =>
                                            void perform(() =>
                                                unwrap(
                                                    event.currentTarget.checked
                                                        ? api.PUT("/api/contacts/{id}/tags/{tagId}", {
                                                              params: { path: { id, tagId: tag.id } },
                                                          })
                                                        : api.DELETE("/api/contacts/{id}/tags/{tagId}", {
                                                              params: { path: { id, tagId: tag.id } },
                                                          }),
                                                ),
                                            )
                                        }
                                    />
                                    {tag.name}
                                </label>
                                <button
                                    className="button button--quiet"
                                    type="button"
                                    onClick={() =>
                                        void perform(() =>
                                            unwrap(
                                                api.DELETE("/api/contacts/tags/{tagId}", {
                                                    params: { path: { tagId: tag.id } },
                                                }),
                                            ),
                                        )
                                    }
                                >
                                    {t("contacts.organize.deleteTag", { name: tag.name })}
                                </button>
                            </li>
                        ))}
                    </ul>
                    {tags.length === 0 && <p className="state state--empty">{t("contacts.organize.noTags")}</p>}
                </fieldset>
            </section>
            <section className="panel" aria-labelledby="contact-folder-heading">
                <h2 id="contact-folder-heading">{t("contacts.organize.folder")}</h2>
                <label>
                    <span>{t("contacts.organize.assignFolder")}</span>
                    <select
                        value={state.folderId ?? ""}
                        onChange={(event) =>
                            void perform(() =>
                                unwrap(
                                    api.PUT("/api/contacts/{id}/folder", {
                                        params: { path: { id } },
                                        body: { folderId: event.currentTarget.value || null },
                                    }),
                                ),
                            )
                        }
                    >
                        <option value="">{t("contacts.organize.unfiled")}</option>
                        {folders.map((folder) => (
                            <option key={folder.id} value={folder.id}>
                                {folder.name}
                            </option>
                        ))}
                    </select>
                </label>
                <form className="grid-form" onSubmit={(event) => void createFolder(event)}>
                    <Field
                        label={t("contacts.organize.newFolder")}
                        value={folderName}
                        onChange={(event) => {
                            setFolderName(event.currentTarget.value);
                        }}
                        required
                    />
                    <label>
                        <span>{t("contacts.organize.parentFolder")}</span>
                        <select
                            value={parentId}
                            onChange={(event) => {
                                setParentId(event.currentTarget.value);
                            }}
                        >
                            <option value="">{t("contacts.organize.rootFolder")}</option>
                            {folders.map((folder) => (
                                <option key={folder.id} value={folder.id}>
                                    {folder.name}
                                </option>
                            ))}
                        </select>
                    </label>
                    <button className="button button--primary" type="submit">
                        {t("contacts.organize.createFolder")}
                    </button>
                </form>
                <ul>
                    {folders.map((folder) => (
                        <li key={folder.id}>
                            <label>
                                <span>{t("contacts.organize.renameFolder", { name: folder.name })}</span>
                                <input
                                    aria-label={t("contacts.organize.renameFolder", { name: folder.name })}
                                    defaultValue={folder.name}
                                    onBlur={(event) => {
                                        const value = event.currentTarget.value.trim();
                                        if (value && value !== folder.name) void updateFolder(folder, { name: value });
                                    }}
                                />
                            </label>
                            <button
                                className="button button--quiet"
                                type="button"
                                aria-label={t("contacts.organize.moveFolderUp", { name: folder.name })}
                                onClick={() =>
                                    void updateFolder(folder, { position: Math.max(0, folder.position - 1) })
                                }
                            >
                                ↑
                            </button>
                            <button
                                className="button button--quiet"
                                type="button"
                                aria-label={t("contacts.organize.moveFolderDown", { name: folder.name })}
                                onClick={() => void updateFolder(folder, { position: folder.position + 1 })}
                            >
                                ↓
                            </button>
                            <label>
                                <span>{t("contacts.organize.parentFolder")}</span>
                                <select
                                    value={folder.parentId ?? ""}
                                    onChange={(event) =>
                                        void updateFolder(folder, { parentId: event.currentTarget.value || null })
                                    }
                                >
                                    <option value="">{t("contacts.organize.rootFolder")}</option>
                                    {folders
                                        .filter((candidate) => candidate.id !== folder.id)
                                        .map((candidate) => (
                                            <option key={candidate.id} value={candidate.id}>
                                                {candidate.name}
                                            </option>
                                        ))}
                                </select>
                            </label>
                            <button
                                className="button button--quiet"
                                type="button"
                                aria-label={t("contacts.organize.deleteFolder", { name: folder.name })}
                                onClick={() =>
                                    void perform(() =>
                                        unwrap(
                                            api.DELETE("/api/contacts/folders/{folderId}", {
                                                params: { path: { folderId: folder.id } },
                                            }),
                                        ),
                                    )
                                }
                            >
                                {t("common.delete")}
                            </button>
                        </li>
                    ))}
                </ul>
                <FormError error={error} />
            </section>
            <Link className="button button--quiet" to="/contacts">
                {t("common.cancel")}
            </Link>
        </>
    );
}
