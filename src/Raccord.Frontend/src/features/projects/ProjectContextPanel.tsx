import { useState, type SubmitEvent } from "react";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError } from "../../shared/ui/Form";
import { Async } from "../../shared/ui/States";
import { formText } from "../../shared/ui/formText";

export function ProjectContextPanel({ projectId, readOnly = false }: { projectId: string; readOnly?: boolean }) {
    const { t, format } = useI18n();
    const context = useQuery(`project-context:${projectId}`, () =>
        unwrap(api.GET("/api/projects/{id}/context", { params: { path: { id: projectId } } })),
    );
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState<unknown>(null);

    async function addEntry(event: SubmitEvent<HTMLFormElement>, kind: "note" | "file") {
        event.preventDefault();
        const form = event.currentTarget;
        const data = new FormData(form);
        setBusy(true);
        setError(null);
        try {
            if (kind === "note") {
                await unwrap(
                    api.POST("/api/projects/{id}/context/notes", {
                        params: { path: { id: projectId } },
                        body: {
                            title: formText(data, "noteTitle"),
                            body: formText(data, "noteBody"),
                            actor: "web",
                        },
                    }),
                );
            } else {
                await unwrap(
                    api.POST("/api/projects/{id}/context/files", {
                        params: { path: { id: projectId } },
                        body: {
                            name: formText(data, "fileName"),
                            folder: null,
                            content: formText(data, "fileContent"),
                            actor: "web",
                        },
                    }),
                );
            }
            form.reset();
            context.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    return (
        <section aria-labelledby="project-context-heading" className="panel">
            <h2 id="project-context-heading">{t("projects.context")}</h2>
            {!readOnly && (
                <div className="grid-form">
                    <form
                        onSubmit={(event) => {
                            void addEntry(event, "note");
                        }}
                    >
                        <h3>{t("projects.notes")}</h3>
                        <Field
                            label={t("projects.noteTitle")}
                            name="noteTitle"
                            required
                            maxLength={200}
                            disabled={busy}
                        />
                        <label className="field">
                            <span>{t("projects.noteBody")}</span>
                            <textarea name="noteBody" required maxLength={10000} disabled={busy} />
                        </label>
                        <button type="submit" className="button button--primary" disabled={busy}>
                            {t("projects.addNote")}
                        </button>
                    </form>
                    <form
                        onSubmit={(event) => {
                            void addEntry(event, "file");
                        }}
                    >
                        <h3>{t("projects.files")}</h3>
                        <Field
                            label={t("projects.fileName")}
                            name="fileName"
                            required
                            maxLength={200}
                            disabled={busy}
                        />
                        <label className="field">
                            <span>{t("projects.fileContent")}</span>
                            <textarea name="fileContent" required maxLength={10000} disabled={busy} />
                        </label>
                        <button type="submit" className="button button--primary" disabled={busy}>
                            {t("projects.addFile")}
                        </button>
                    </form>
                </div>
            )}
            <FormError error={error} />
            <Async query={context}>
                {(value) => (
                    <ul aria-label={t("projects.contextEntries")}>
                        {value.entries
                            .filter((entry) => entry.kind === "note" || entry.kind === "file")
                            .map((entry) => (
                                <li key={entry.id}>
                                    <strong>{entry.name}</strong>
                                    {entry.content && <p>{entry.content}</p>}
                                    <small>{format.dateTime(entry.createdAt)}</small>
                                </li>
                            ))}
                    </ul>
                )}
            </Async>
        </section>
    );
}
