<!-- pdac-scope: none reason="This change affects navigation in the internal engineering evidence report and does not alter product behavior." -->

## Why

The engineering evidence report groups journeys by bounded context, but long groups make the navigation difficult to scan and require excessive scrolling, especially on narrow screens. Reviewers need to fold away areas they are not inspecting while keeping each area easy to reopen.

## What Changes

- Make each bounded-context, Cross-cutting, and Unclassified navigation group independently collapsible.
- Keep groups expanded on first load and preserve all journey, status, evidence, filter, and history links.
- Add regression coverage for the generated static report.

## Capabilities

### New Capabilities

None. This change only improves an internal engineering evidence report.

### Modified Capabilities

None. No product behavior or product capability requirement changes.

## Impact

`tools/static-site/static-site.mjs`, `tools/static-site/static-site.test.mjs`, and `docs/operations/process-ledger.md`. The report remains static and adds no runtime dependency. Product APIs, data, and deployment configuration are unaffected.
