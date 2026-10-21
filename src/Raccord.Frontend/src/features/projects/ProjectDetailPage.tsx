import { useState, type SubmitEvent } from "react";
import { useNavigate, useParams } from "react-router";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError, SelectField } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async, Empty } from "../../shared/ui/States";
import { AddBillableLineForm } from "./AddBillableLineForm";
import { ProjectTimeSchedule } from "./ProjectTimeSchedule";
import { ProjectContextPanel } from "./ProjectContextPanel";
import { formText } from "../../shared/ui/formText";

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
    const [customerId, setCustomerId] = useState(project.customerId);
    const [contactPersonId, setContactPersonId] = useState(project.contactPersonId ?? "");
    const [accountManagerId, setAccountManagerId] = useState(project.accountManagerId ?? "");
    const contactPeople = useQuery(`contact-people:${customerId}`, () =>
        unwrap(api.GET("/api/contacts/{id}/people", { params: { path: { id: customerId } } })),
    );
    const customers = useQuery("customers", () => unwrap(api.GET("/api/customers")));
    const projectTypes = useQuery("project-types", () => unwrap(api.GET("/api/project-coordination/types")));
    const projectTeams = useQuery("project-teams", () => unwrap(api.GET("/api/project-coordination/teams")));
    const accountManagers = useQuery("project-account-managers", () =>
        unwrap(api.GET("/api/projects/account-managers")),
    );
    const [error, setError] = useState<unknown>(null);
    const [detailsError, setDetailsError] = useState<unknown>(null);
    const [cancellationPreview, setCancellationPreview] = useState<
        Schemas["ProjectCancellationPreviewResponse"] | null
    >(null);
    const [cancellationError, setCancellationError] = useState<unknown>(null);
    const [completeOpenTasks, setCompleteOpenTasks] = useState(false);
    const [removePlannedResources, setRemovePlannedResources] = useState(false);
    const [withdrawInvitations, setWithdrawInvitations] = useState(false);

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

    async function saveDetails(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        setBusy(true);
        setDetailsError(null);
        try {
            await unwrap(
                api.PUT("/api/projects/{id}", {
                    params: { path: { id: project.id } },
                    body: {
                        customerId: formText(data, "customerId"),
                        name: formText(data, "name"),
                        startDate: formText(data, "startDate"),
                        endDate: formText(data, "endDate"),
                        externalReference: formText(data, "externalReference") || null,
                        projectTypeId: formText(data, "projectTypeId") || null,
                        teamId: formText(data, "teamId") || null,
                        contactPersonId: contactPersonId || null,
                        accountManagerId: accountManagerId || null,
                        projectNumber: formText(data, "projectNumber"),
                    },
                }),
            );
            onChanged();
        } catch (failure) {
            setDetailsError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function openCancellation() {
        setCancellationError(null);
        try {
            const preview = await unwrap(
                api.GET("/api/projects/{id}/cancellation-preview", {
                    params: { path: { id: project.id } },
                }),
            );
            setCancellationPreview(preview);
            setCompleteOpenTasks(false);
            setRemovePlannedResources(false);
            setWithdrawInvitations(false);
        } catch (failure) {
            setCancellationError(failure);
        }
    }

    async function confirmCancellation() {
        setBusy(true);
        setCancellationError(null);
        try {
            await unwrap(
                api.POST("/api/projects/{id}/cancellation", {
                    params: { path: { id: project.id } },
                    body: {
                        confirm: true,
                        completeOpenTasks,
                        removePlannedResources,
                        withdrawInvitations,
                    },
                }),
            );
            setCancellationPreview(null);
            onChanged();
        } catch (failure) {
            setCancellationError(failure);
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

            <section aria-labelledby="project-details-heading" className="panel">
                <h2 id="project-details-heading">{t("projects.details")}</h2>
                <form
                    className="grid-form"
                    onSubmit={(event) => {
                        void saveDetails(event);
                    }}
                >
                    <Field
                        label={t("projects.name")}
                        name="name"
                        required
                        maxLength={200}
                        defaultValue={project.name}
                        disabled={busy || project.status === "complete"}
                    />
                    <Field
                        label={t("projects.number")}
                        name="projectNumber"
                        required
                        maxLength={64}
                        defaultValue={project.projectNumber}
                        disabled={busy || project.status === "complete"}
                    />
                    <Async query={customers}>
                        {(list) => (
                            <SelectField
                                label={t("common.customer")}
                                name="customerId"
                                required
                                value={customerId}
                                onChange={(event) => {
                                    setCustomerId(event.target.value);
                                    setContactPersonId("");
                                }}
                                disabled={busy || project.status === "complete"}
                            >
                                {list.map((customer) => (
                                    <option key={customer.id} value={customer.id}>
                                        {customer.legalName}
                                    </option>
                                ))}
                            </SelectField>
                        )}
                    </Async>
                    <Async query={projectTypes}>
                        {(types) => (
                            <SelectField
                                label={t("projects.projectType")}
                                name="projectTypeId"
                                defaultValue={project.projectTypeId ?? ""}
                                disabled={busy || project.status === "complete"}
                            >
                                <option value="">{t("projects.noProjectType")}</option>
                                {types.map((type) => (
                                    <option key={type.id} value={type.id}>
                                        {type.name}
                                    </option>
                                ))}
                            </SelectField>
                        )}
                    </Async>
                    <Async query={projectTeams}>
                        {(teams) => (
                            <SelectField
                                label={t("projects.team")}
                                name="teamId"
                                defaultValue={project.teamId ?? ""}
                                disabled={busy || project.status === "complete"}
                            >
                                <option value="">{t("projects.noTeam")}</option>
                                {teams.map((team) => (
                                    <option key={team.id} value={team.id}>
                                        {team.name}
                                    </option>
                                ))}
                            </SelectField>
                        )}
                    </Async>
                    <Async query={contactPeople}>
                        {(people) => (
                            <SelectField
                                label={t("projects.contactPerson")}
                                name="contactPersonId"
                                value={contactPersonId}
                                onChange={(event) => {
                                    setContactPersonId(event.target.value);
                                }}
                                disabled={busy || project.status === "complete"}
                            >
                                <option value="">{t("projects.noContactPerson")}</option>
                                {people.map((person) => (
                                    <option key={person.id} value={person.id}>
                                        {person.name}
                                    </option>
                                ))}
                            </SelectField>
                        )}
                    </Async>
                    <Async query={accountManagers}>
                        {(managers) => (
                            <SelectField
                                label={t("projects.accountManager")}
                                name="accountManagerId"
                                value={accountManagerId}
                                onChange={(event) => {
                                    setAccountManagerId(event.target.value);
                                }}
                                disabled={busy || project.status === "complete"}
                            >
                                <option value="">{t("projects.noAccountManager")}</option>
                                {managers.map((manager) => (
                                    <option key={manager.id} value={manager.id}>
                                        {manager.displayName}
                                    </option>
                                ))}
                            </SelectField>
                        )}
                    </Async>
                    <Field
                        label={t("projects.startDate")}
                        name="startDate"
                        type="date"
                        required
                        defaultValue={project.startDate}
                        disabled={busy || project.status === "complete"}
                    />
                    <Field
                        label={t("projects.endDate")}
                        name="endDate"
                        type="date"
                        required
                        defaultValue={project.endDate}
                        disabled={busy || project.status === "complete"}
                    />
                    <Field
                        label={t("projects.externalReference")}
                        name="externalReference"
                        maxLength={200}
                        defaultValue={project.externalReference ?? ""}
                        disabled={busy || project.status === "complete"}
                    />
                    <div className="actions">
                        <button
                            type="submit"
                            className="button button--primary"
                            disabled={busy || project.status === "complete"}
                        >
                            {t("projects.saveDetails")}
                        </button>
                    </div>
                    <FormError error={detailsError} />
                </form>
            </section>

            <ProjectTimeSchedule projectId={project.id} readOnly={project.status === "complete"} />
            <ProjectContextPanel projectId={project.id} readOnly={project.status === "complete"} />

            <section aria-labelledby="project-status-heading" className="panel">
                <h2 id="project-status-heading">{t("projects.status")}</h2>
                <div className="actions">
                    {project.status === "cancelled" ? (
                        <p>{t("projects.status.cancelled")}</p>
                    ) : (
                        <>
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
                        </>
                    )}
                </div>
                <FormError error={error} />
            </section>

            {project.hasCancellationWarning && (
                <p className="notice notice--warning" role="alert">
                    {t("projects.cancellationWarning")}
                </p>
            )}
            {project.status !== "cancelled" && (
                <div className="actions">
                    <button
                        type="button"
                        className="button button--quiet"
                        onClick={() => {
                            void openCancellation();
                        }}
                    >
                        {t("projects.cancelProject")}
                    </button>
                </div>
            )}
            {cancellationPreview && (
                <section role="dialog" aria-modal="true" aria-labelledby="cancellation-heading" className="panel">
                    <h2 id="cancellation-heading">{t("projects.cancellationConfirmTitle")}</h2>
                    {cancellationPreview.openTaskCount +
                        cancellationPreview.plannedCrewCount +
                        cancellationPreview.plannedTransportCount >
                    0 ? (
                        <p role="alert">
                            {t("projects.cancellationOpenItems", {
                                tasks: cancellationPreview.openTaskCount,
                                crew: cancellationPreview.plannedCrewCount,
                                transport: cancellationPreview.plannedTransportCount,
                            })}
                        </p>
                    ) : (
                        <p>{t("projects.cancellationNoOpenItems")}</p>
                    )}
                    <p>{t("projects.cancellationConfirmHelp")}</p>
                    {cancellationPreview.activeInvitationCount > 0 ||
                    cancellationPreview.plannedCrewCount > 0 ||
                    cancellationPreview.plannedTransportCount > 0 ||
                    (cancellationPreview.canCompleteOpenTasks && cancellationPreview.openTaskCount > 0) ? (
                        <fieldset>
                            <legend>{t("projects.cancellationActions")}</legend>
                            {cancellationPreview.canCompleteOpenTasks && cancellationPreview.openTaskCount > 0 && (
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={completeOpenTasks}
                                        onChange={(event) => {
                                            setCompleteOpenTasks(event.target.checked);
                                        }}
                                    />
                                    {t("projects.cancellationCompleteTasks", {
                                        count: cancellationPreview.openTaskCount,
                                    })}
                                </label>
                            )}
                            {(cancellationPreview.plannedCrewCount > 0 ||
                                cancellationPreview.plannedTransportCount > 0) && (
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={removePlannedResources}
                                        onChange={(event) => {
                                            setRemovePlannedResources(event.target.checked);
                                        }}
                                    />
                                    {t("projects.cancellationRemoveResources", {
                                        crew: cancellationPreview.plannedCrewCount,
                                        transport: cancellationPreview.plannedTransportCount,
                                    })}
                                </label>
                            )}
                            {cancellationPreview.activeInvitationCount > 0 && (
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={withdrawInvitations}
                                        onChange={(event) => {
                                            setWithdrawInvitations(event.target.checked);
                                        }}
                                    />
                                    {t("projects.cancellationWithdrawInvitations", {
                                        count: cancellationPreview.activeInvitationCount,
                                    })}
                                </label>
                            )}
                        </fieldset>
                    ) : null}
                    <div className="actions">
                        <button
                            type="button"
                            className="button button--danger"
                            disabled={busy}
                            onClick={() => void confirmCancellation()}
                        >
                            {t("projects.confirmCancellation")}
                        </button>
                        <button
                            type="button"
                            className="button button--quiet"
                            disabled={busy}
                            onClick={() => {
                                setCancellationPreview(null);
                            }}
                        >
                            {t("common.cancel")}
                        </button>
                    </div>
                    <FormError error={cancellationError} />
                </section>
            )}

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

            <AddBillableLineForm projectId={project.id} onAdded={onChanged} readOnly={project.status === "complete"} />
        </>
    );
}
