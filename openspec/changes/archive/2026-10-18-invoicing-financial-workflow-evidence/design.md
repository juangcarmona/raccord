<!-- pdac-scope: none reason="This design describes test infrastructure only; it makes no product-semantic decision." -->

## Context

See proposal.md - Why. The existing Web test suite covers these routes with PostgreSQL, while the existing AppHost selection covers invoice issuance, documents and structured e-invoice flows. `BrowserJourneyBase` already provides synthetic customer, project, billable-line and invoice setup helpers.

## Goals / Non-Goals

**Goals:** Exercise supported invoicing collection and reporting routes through the booted Aspire AppHost with synthetic records; preserve accurate API evidence mode and bounded-context grouping.

**Non-Goals:** Change financial behavior, assert legal or tax compliance, use real customer data, or add unsupported staged billing or rectification behavior.

## Decisions

- Add one API-mode AppHost journey that creates a synthetic issued invoice, records partial and final payments, transitions a refundable deposit, creates and lists a reminder for an overdue invoice, and asserts finance report, ledger and CSV exports. Tag the result with both BC-INVOICING and BC-COMMERCIAL because the source cases span those canonical bounded contexts.
- Use existing contract types and routes. Do not duplicate persistence assertions from web tests or add database access to the E2E test.
- Add the journey to `per-pr-selection.txt`; metadata and report generation will then retain its trace under the Invoicing bounded-context group without browser screenshots that do not demonstrate API behavior.
- Keep the selected battery and all existing invoice safety checks unchanged.

## Risks / Trade-offs

- Shared AppHost state can make financial totals unstable → scope assertions to a deterministic issue-date window and the synthetic invoice/customer created by this journey.
- The reminder requires an overdue balance → issue the synthetic invoice on a fixed past date and leave an outstanding amount until after reminder history is asserted.

## Migration Plan

No migration or deployment change is required. The new journey runs as part of the existing selected AppHost suite.
