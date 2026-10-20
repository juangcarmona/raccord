import { useState } from "react";
import { useNavigate, useParams } from "react-router";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { FormError, SelectField } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async, Empty } from "../../shared/ui/States";
import { AddBillableLineForm } from "./AddBillableLineForm";

const projectStatuses = [
    { value: "inquiry", label: "projects.status.inquiry" },
    { value: "draft", label: "projects.status.draft" },
    { value: "pending", label: "projects.status.pending" },
    { value: "confirmed", label: "projects.status.confirmed" },
    { value: "in-progress", label: "projects.status.in-progress" },
    { value: "execution-done", label: "projects.status.execution-done" },
    { value: "complete", label: "projects.status.complete" },
] as const;

export function ProjectDetailPage() {
    const { id = "" } = useParams();
    const query = useQuery(`project:${id}`, () => unwrap(api.GET("/api/projects/{id}", { params: { path: { id } } })));
    return <Async query={query}>{(project) => <ProjectView project={project} onChanged={query.reload} />}</Async>;
}

function ProjectView({ project, onChanged }: { project: Schemas["ProjectResponse"]; onChanged: () => void }) {
    const { t, format, describe } = useI18n();
    const navigate = useNavigate();
    const [selected, setSelected] = useState<ReadonlySet<string>>(new Set());
    const [busy, setBusy] = useState(false);
    const [status, setStatus] = useState(project.status);
    const [error, setError] = useState<unknown>(null);

    function toggle(lineId: string) {
        setSelected((current) => {
            const next = new Set(current);
            if (!next.delete(lineId)) next.add(lineId);
            return next;
        });
    }

    async function createDraft() {
        setBusy(true);
        setError(null);
        try {
            const draft = await unwrap(
                api.POST("/api/invoice-drafts", {
                    body: { projectId: project.id, billableLineIds: [...selected], seriesCode: null },
                }),
            );
            await navigate(`/drafts/${draft.id}`);
        } catch (failure) {
            setError(failure);
            setBusy(false);
        }
    }

    async function saveStatus() {
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.PUT("/api/projects/{id}/status", {
                    params: { path: { id: project.id } },
                    body: { status },
                }),
            );
            onChanged();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    return (
        <>
            <PageHeader title={project.name}>
                <p className="subtitle">
                    {project.customerName} · {format.date(project.startDate)} – {format.date(project.endDate)}
                </p>
            </PageHeader>

            <section aria-labelledby="project-status-heading" className="panel">
                <h2 id="project-status-heading">{t("projects.status")}</h2>
                <div className="actions">
                    <SelectField
                        label={t("projects.status")}
                        name="projectStatus"
                        value={status}
                        onChange={(event) => {
                            setStatus(event.target.value);
                        }}
                    >
                        {projectStatuses.map(({ value, label }) => (
                            <option key={value} value={value}>
                                {t(label)}
                            </option>
                        ))}
                    </SelectField>
                    <button
                        type="button"
                        className="button button--primary"
                        disabled={busy || status === project.status}
                        onClick={() => void saveStatus()}
                    >
                        {t("common.save")}
                    </button>
                </div>
                <FormError error={error} />
            </section>

            <section aria-labelledby="lines-heading">
                <h2 id="lines-heading">{t("projects.lines")}</h2>
                {project.billableLines.length === 0 ? (
                    <Empty>{t("projects.linesEmpty")}</Empty>
                ) : (
                    <div className="table-scroll">
                        <table>
                            <caption className="visually-hidden">{t("projects.lines")}</caption>
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <span className="visually-hidden">{t("common.actions")}</span>
                                    </th>
                                    <th scope="col">{t("common.description")}</th>
                                    <th scope="col">{t("projects.kind")}</th>
                                    <th scope="col" className="num">
                                        {t("common.quantity")}
                                    </th>
                                    <th scope="col" className="num">
                                        {t("common.unitPrice")}
                                    </th>
                                    <th scope="col" className="num">
                                        {t("common.vat")}
                                    </th>
                                    <th scope="col" className="num">
                                        {t("common.netAmount")}
                                    </th>
                                    <th scope="col">{t("common.status")}</th>
                                </tr>
                            </thead>
                            <tbody>
                                {project.billableLines.map((line) => (
                                    <tr key={line.id}>
                                        <td>
                                            <input
                                                type="checkbox"
                                                aria-label={t("projects.selectLine", { description: line.description })}
                                                disabled={line.billingState !== "unbilled" || busy}
                                                checked={selected.has(line.id)}
                                                onChange={() => {
                                                    toggle(line.id);
                                                }}
                                            />
                                        </td>
                                        <td>{line.description}</td>
                                        <td>{describe("kind", line.kind)}</td>
                                        <td className="num">{format.quantity(line.quantity)}</td>
                                        <td className="num">{format.money(line.unitPrice, project.currency)}</td>
                                        <td className="num">{format.percent(line.vatRate)}</td>
                                        <td className="num">{format.money(line.netAmount, project.currency)}</td>
                                        <td>
                                            <span className={`badge badge--${line.billingState}`}>
                                                {describe("billing", line.billingState)}
                                            </span>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
                <div className="actions">
                    <button
                        type="button"
                        className="button button--primary"
                        disabled={selected.size === 0 || busy}
                        onClick={() => {
                            void createDraft();
                        }}
                    >
                        {t("projects.createDraft", { count: selected.size })}
                    </button>
                </div>
                <FormError error={error} />
            </section>

            <AddBillableLineForm projectId={project.id} onAdded={onChanged} />
        </>
    );
}
