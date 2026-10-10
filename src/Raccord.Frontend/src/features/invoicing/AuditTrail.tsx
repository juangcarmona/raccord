import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Async, Empty } from "../../shared/ui/States";

export function AuditTrail({ entityId }: { entityId: string }) {
    const { t, format, describe } = useI18n();
    const query = useQuery(`audit:${entityId}`, () =>
        unwrap(api.GET("/api/audit-events", { params: { query: { entityId } } })),
    );
    return (
        <section aria-labelledby="audit-heading">
            <h2 id="audit-heading">{t("audit.title")}</h2>
            <Async query={query}>
                {(events) =>
                    events.length === 0 ? (
                        <Empty>{t("audit.empty")}</Empty>
                    ) : (
                        <ol className="audit">
                            {events.map((event) => (
                                <li key={event.id}>
                                    <time dateTime={event.occurredAt}>{format.dateTime(event.occurredAt)}</time>{" "}
                                    <strong>{describe("audit", event.action, event.action)}</strong> · {event.actor}
                                </li>
                            ))}
                        </ol>
                    )
                }
            </Async>
        </section>
    );
}
