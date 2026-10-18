<!-- pdac-scope: none reason="This design describes test-only verification of existing behavior and makes no product-semantic decision." -->

## Context

`InvoicePage` renders the existing `AuditTrail` component for the issued invoice. The component requests audit events for the invoice and displays each event's localized action, timestamp and actor. Invoice finalization records `invoice.issued` and `invoice.document.stored` events. Existing integration tests cover audit persistence and immutability; this change covers only the rendered browser workflow.

## Decisions

- Reuse `BrowserJourneyBase` to create a unique synthetic customer, project and billable line, then finalize the invoice through the application UI in Aspire.
- Assert the visible trail contains the invoice-issued and document-archived events in order, each with the authenticated test actor and a parseable `time[datetime]` value.
- Tag the journey with both Compliance and audit and Invoicing so evidence appears in both bounded-context groups. Keep it in the committed per-PR manifest.
- Leave immutable-event triggers, content-hash integrity, finalization atomicity and concurrency to database/domain integration tests; browser behavior does not stand in for those proofs.
- Make no assertions about tax-authority submission, legal interpretation or VERI*FACTU compliance.

## Risks / Trade-offs

- The invoice page can load before the audit query completes, so the journey waits for the expected second event before checking the complete list.
- The test relies on the shared authenticated test identity; asserting its actor makes identity regressions visible in the displayed trail.
- Success artifacts contain only synthetic invoice data and remain owned by the journey under both bounded contexts.
