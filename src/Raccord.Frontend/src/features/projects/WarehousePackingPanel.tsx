import { useState } from "react";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import type { MessageKey } from "../../shared/i18n/messages";
import { FormError } from "../../shared/ui/Form";
import { Async, Empty } from "../../shared/ui/States";

type PackingAction = "pack" | "book-out" | "receive";

export function WarehousePackingPanel({
    project,
    onChanged,
}: {
    project: Schemas["ProjectResponse"];
    onChanged: () => void;
}) {
    const { t } = useI18n();
    const hasLines = (project.packingLines ?? []).length > 0;
    const equipment = useQuery(`equipment-items:${hasLines}`, () =>
        hasLines ? unwrap(api.GET("/api/equipment/items")) : Promise.resolve([]),
    );

    return (
        <section aria-labelledby="warehouse-heading">
            <h2 id="warehouse-heading">{t("warehouse.title")}</h2>
            <p>{t("warehouse.manualIntro")}</p>
            <Async query={equipment}>
                {(items) => <PackingLines project={project} items={items} onChanged={onChanged} />}
            </Async>
        </section>
    );
}

function PackingLines({
    project,
    items,
    onChanged,
}: {
    project: Schemas["ProjectResponse"];
    items: Schemas["EquipmentItemResponse"][];
    onChanged: () => void;
}) {
    const { t } = useI18n();
    const [busyLine, setBusyLine] = useState<string | null>(null);
    const [error, setError] = useState<unknown>(null);
    const lines = project.packingLines ?? [];
    const names = new Map(items.map((item) => [item.id, `${item.code} · ${item.name}`]));

    async function transition(line: Schemas["PackingLineResponse"], action: PackingAction) {
        setBusyLine(line.id);
        setError(null);
        try {
            if (action === "pack") {
                await unwrap(
                    api.POST("/api/projects/{id}/packing-lines/{lineId}/pack", {
                        params: { path: { id: project.id, lineId: line.id } },
                        body: { actor: "warehouse-ui" },
                    }),
                );
            } else if (action === "book-out") {
                await unwrap(
                    api.POST("/api/projects/{id}/packing-lines/{lineId}/book-out", {
                        params: { path: { id: project.id, lineId: line.id } },
                        body: { actor: "warehouse-ui", scanCode: null },
                    }),
                );
            } else {
                await unwrap(
                    api.POST("/api/projects/{id}/packing-lines/{lineId}/receive", {
                        params: { path: { id: project.id, lineId: line.id } },
                        body: { actor: "warehouse-ui" },
                    }),
                );
            }
            onChanged();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusyLine(null);
        }
    }

    return (
        <>
            {lines.length === 0 ? (
                <Empty>{t("warehouse.empty")}</Empty>
            ) : (
                <div className="table-scroll">
                    <table>
                        <caption className="visually-hidden">{t("warehouse.title")}</caption>
                        <thead>
                            <tr>
                                <th scope="col">{t("warehouse.item")}</th>
                                <th scope="col" className="num">
                                    {t("common.quantity")}
                                </th>
                                <th scope="col">{t("warehouse.status")}</th>
                                <th scope="col">{t("common.actions")}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {lines.map((line) => (
                                <tr key={line.id}>
                                    <th scope="row">{names.get(line.equipmentId) ?? line.equipmentId}</th>
                                    <td className="num">{line.quantity}</td>
                                    <td>{statusFor(line, t)}</td>
                                    <td>
                                        <div className="actions">
                                            {!line.isPacked && (
                                                <button
                                                    type="button"
                                                    className="button"
                                                    disabled={busyLine !== null}
                                                    onClick={() => void transition(line, "pack")}
                                                >
                                                    {t("warehouse.pack")}
                                                </button>
                                            )}
                                            {line.isPacked && !line.isBookedOut && (
                                                <button
                                                    type="button"
                                                    className="button button--primary"
                                                    disabled={busyLine !== null}
                                                    onClick={() => void transition(line, "book-out")}
                                                >
                                                    {t("warehouse.bookOut")}
                                                </button>
                                            )}
                                            {line.isBookedOut && !line.isReceived && (
                                                <button
                                                    type="button"
                                                    className="button"
                                                    disabled={busyLine !== null}
                                                    onClick={() => void transition(line, "receive")}
                                                >
                                                    {t("warehouse.receive")}
                                                </button>
                                            )}
                                        </div>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
            <FormError error={error} />
        </>
    );
}

function statusFor(line: Schemas["PackingLineResponse"], t: (key: MessageKey) => string): string {
    if (line.isReceived) return t("warehouse.statusReceived");
    if (line.isBookedOut) return t("warehouse.statusBookedOut");
    if (line.isPacked) return t("warehouse.statusPacked");
    return t("warehouse.statusPlanned");
}
