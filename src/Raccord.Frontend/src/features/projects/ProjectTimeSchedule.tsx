import { useState, type SubmitEvent } from "react";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError } from "../../shared/ui/Form";

export function ProjectTimeSchedule({ projectId, readOnly }: { projectId: string; readOnly: boolean }) {
    const { t } = useI18n();
    const query = useQuery(`project-periods:${projectId}`, () =>
        unwrap(api.GET("/api/projects/{id}/periods", { params: { path: { id: projectId } } })),
    );
    const functions = useQuery(`project-functions:${projectId}`, () =>
        unwrap(api.GET("/api/projects/{id}/functions", { params: { path: { id: projectId } } })),
    );
    const [name, setName] = useState("");
    const [selectedPeriods, setSelectedPeriods] = useState<ReadonlySet<string>>(new Set());
    const [selectedKinds, setSelectedKinds] = useState<ReadonlySet<number>>(new Set());
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState<unknown>(null);

    async function addPeriod(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.POST("/api/projects/{id}/periods", {
                    params: { path: { id: projectId } },
                    body: { name },
                }),
            );
            setName("");
            query.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function updatePeriod(period: Schemas["ProjectTimePeriodResponse"], data: FormData) {
        setBusy(true);
        setError(null);
        try {
            const periodName = data.get("name");
            await unwrap(
                api.PUT("/api/projects/{id}/periods/{periodId}", {
                    params: { path: { id: projectId, periodId: period.id } },
                    body: {
                        name: typeof periodName === "string" ? periodName : "",
                        start: asOffset(data.get("start")),
                        end: asOffset(data.get("end")),
                        showOnInternalDocuments: data.has("internal"),
                        showOnExternalDocuments: data.has("external"),
                    },
                }),
            );
            query.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function movePeriod(index: number, direction: -1 | 1) {
        if (query.status !== "success") return;
        const periods = [...query.data];
        const target = index + direction;
        if (target < 0 || target >= periods.length) return;
        const current = periods[index];
        const neighbor = periods[target];
        if (!current || !neighbor) return;
        periods[index] = neighbor;
        periods[target] = current;
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.PUT("/api/projects/{id}/periods/order", {
                    params: { path: { id: projectId } },
                    body: { periodIds: periods.map((period) => period.id) },
                }),
            );
            query.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function duplicatePeriod(periodId: string) {
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.POST("/api/projects/{id}/periods/{periodId}/duplicate", {
                    params: { path: { id: projectId, periodId } },
                }),
            );
            query.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function designatePeriod(periodId: string, planning: boolean, usage: boolean) {
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.PUT("/api/projects/{id}/periods/{periodId}/designation", {
                    params: { path: { id: projectId, periodId } },
                    body: { isDefaultPlanning: planning, isDefaultUsage: usage },
                }),
            );
            query.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    async function generateFunctions() {
        setBusy(true);
        setError(null);
        try {
            await unwrap(
                api.POST("/api/projects/{id}/functions/from-periods", {
                    params: { path: { id: projectId } },
                    body: {
                        periodIds: [...selectedPeriods],
                        kinds: [...selectedKinds],
                    },
                }),
            );
            setSelectedPeriods(new Set());
            setSelectedKinds(new Set());
            functions.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    function toggleSelection<T extends string | number>(current: ReadonlySet<T>, value: T) {
        const next = new Set(current);
        if (!next.delete(value)) next.add(value);
        return next;
    }

    return (
        <section aria-labelledby="project-time-schedule-heading" className="panel">
            <h2 id="project-time-schedule-heading">{t("projects.schedule")}</h2>
            {query.status === "loading" ? <p>{t("state.loading")}</p> : null}
            {query.status === "error" ? <FormError error={query.error} /> : null}
            {query.status === "success" ? (
                <ol aria-label={t("projects.schedule")}>
                    {query.data.map((period, index) => (
                        <li key={period.id}>
                            <form
                                className="grid-form"
                                aria-label={t("projects.periodForm", { name: period.name })}
                                onSubmit={(event) => {
                                    event.preventDefault();
                                    void updatePeriod(period, new FormData(event.currentTarget));
                                }}
                            >
                                <Field
                                    label={t("projects.periodName")}
                                    name="name"
                                    required
                                    defaultValue={period.name}
                                    disabled={readOnly || busy}
                                />
                                <Field
                                    label={t("projects.periodStart")}
                                    name="start"
                                    type="datetime-local"
                                    defaultValue={asLocal(period.start)}
                                    disabled={readOnly || busy}
                                />
                                <Field
                                    label={t("projects.periodEnd")}
                                    name="end"
                                    type="datetime-local"
                                    defaultValue={asLocal(period.end)}
                                    disabled={readOnly || busy}
                                />
                                <label>
                                    <input
                                        type="checkbox"
                                        name="internal"
                                        defaultChecked={period.showOnInternalDocuments}
                                        disabled={readOnly || busy}
                                    />
                                    {t("projects.periodInternal")}
                                </label>
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={selectedPeriods.has(period.id)}
                                        disabled={readOnly || busy || !period.start || !period.end}
                                        onChange={() => {
                                            setSelectedPeriods((current) => toggleSelection(current, period.id));
                                        }}
                                    />
                                    {t("projects.selectPeriodForFunctions", { name: period.name })}
                                </label>
                                <label>
                                    <input
                                        type="checkbox"
                                        name="external"
                                        defaultChecked={period.showOnExternalDocuments}
                                        disabled={readOnly || busy}
                                    />
                                    {t("projects.periodExternal")}
                                </label>
                                <div className="actions">
                                    <button
                                        type="submit"
                                        className="button button--primary"
                                        disabled={readOnly || busy}
                                    >
                                        {t("common.save")}
                                    </button>
                                    <button
                                        type="button"
                                        className="button button--quiet"
                                        disabled={readOnly || busy}
                                        onClick={() => void duplicatePeriod(period.id)}
                                    >
                                        {t("projects.duplicatePeriod")}
                                    </button>
                                    <button
                                        type="button"
                                        className="button button--quiet"
                                        disabled={readOnly || busy || index === 0}
                                        onClick={() => void movePeriod(index, -1)}
                                    >
                                        {t("projects.movePeriodUp")}
                                    </button>
                                    <button
                                        type="button"
                                        className="button button--quiet"
                                        disabled={readOnly || busy || index === query.data.length - 1}
                                        onClick={() => void movePeriod(index, 1)}
                                    >
                                        {t("projects.movePeriodDown")}
                                    </button>
                                    <button
                                        type="button"
                                        className="button button--quiet"
                                        disabled={readOnly || busy || period.isDefaultPlanning}
                                        onClick={() => void designatePeriod(period.id, true, false)}
                                    >
                                        {t("projects.designatePlanning")}
                                    </button>
                                    <button
                                        type="button"
                                        className="button button--quiet"
                                        disabled={readOnly || busy || period.isDefaultUsage}
                                        onClick={() => void designatePeriod(period.id, false, true)}
                                    >
                                        {t("projects.designateUsage")}
                                    </button>
                                </div>
                            </form>
                        </li>
                    ))}
                </ol>
            ) : null}
            <form
                className="actions"
                onSubmit={(event) => {
                    void addPeriod(event);
                }}
            >
                <Field
                    label={t("projects.newPeriodName")}
                    name="newPeriodName"
                    required
                    value={name}
                    disabled={readOnly || busy}
                    onChange={(event) => {
                        setName(event.target.value);
                    }}
                />
                <button type="submit" className="button button--primary" disabled={readOnly || busy || !name.trim()}>
                    {t("projects.addPeriod")}
                </button>
            </form>
            <fieldset className="actions" aria-label={t("projects.generateFunctions")}>
                <legend>{t("projects.generateFunctions")}</legend>
                {[0, 1].map((kind) => (
                    <label key={kind}>
                        <input
                            type="checkbox"
                            checked={selectedKinds.has(kind)}
                            disabled={readOnly || busy}
                            onChange={() => {
                                setSelectedKinds((current) => toggleSelection(current, kind));
                            }}
                        />
                        {kind === 0 ? t("projects.crewFunctions") : t("projects.transportFunctions")}
                    </label>
                ))}
                <button
                    type="button"
                    className="button button--primary"
                    disabled={readOnly || busy || selectedPeriods.size === 0 || selectedKinds.size === 0}
                    onClick={() => void generateFunctions()}
                >
                    {t("projects.createSelectedFunctions")}
                </button>
            </fieldset>
            {functions.status === "success" ? (
                <ul aria-label={t("projects.projectFunctions")}>
                    {functions.data.map((functionItem) => (
                        <li key={functionItem.id}>{functionItem.name}</li>
                    ))}
                </ul>
            ) : null}
            <FormError error={error} />
        </section>
    );
}

function asOffset(value: FormDataEntryValue | null): string | null {
    return typeof value === "string" && value ? new Date(value).toISOString() : null;
}

function asLocal(value: string | null | undefined): string {
    return value ? new Date(value).toISOString().slice(0, 16) : "";
}
