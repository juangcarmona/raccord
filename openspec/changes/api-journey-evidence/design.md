<!-- pdac-scope: none reason="This design changes E2E evidence classification and rendering, not product behavior." -->

## Context

The current `BrowserJourneyBase.RunJourneyAsync` always captures a success screenshot and video when visual evidence is enabled, then writes a result manifest containing only the bounded-context areas. The static report builder already understands an `evidenceMode: api` field and shows an API-specific note, but the writer never emits that field. Several API-only journeys call `Page.SetContentAsync` with a fabricated success heading before the shared screenshot, while others capture the generic homepage or save screenshots outside the shared evidence directory.

## Goals / Non-Goals

**Goals:** make each API-only result explicit, prevent API test output or unrelated pages from appearing as product UI, preserve report grouping by bounded context, and keep genuine UI evidence for journeys that exercise the interface.

**Non-Goals:** change API assertions, implement missing product screens, alter test selection, change test verdicts, or publish evidence to a different audience.

## Decisions

- Add a canonical `EvidenceMode` trait axis with `browser` as the default and `api` as the explicit API-only value. Add API mode to every selected API-only journey class; keep mixed journeys in browser mode when they exercise and assert a real UI flow.
- Resolve the evidence mode once per journey run. API mode writes `evidenceMode: "api"` into the existing result JSON, omits success/failure screenshots and disables video recording when creating the browser context, and retains the Playwright trace and pass/fail result.
- Keep failure capture best-effort and verdict-preserving. API-mode failures save the trace without screenshotting a test-authored or unrelated page.
- Keep the report's bounded-context navigation unchanged. For API results, display the explicit API/test evidence note, retain trace links, and suppress screenshot/video assets even if a stray visual file is present.
- Remove the synthetic `SetContentAsync` pages and direct screenshots from API-only journeys. For mixed UI/API journeys, capture the actual application screen after the tested UI action and remove any fake result page or nonstandard screenshot path.
- Use the existing shared evidence directory and result-manifest ownership for any supplemental screenshot that remains; do not add context guesses from filenames.

Alternatives considered: add another report category separate from journey evidence (rejected because the existing bounded-context navigation already provides discoverability); retain fabricated HTML as “API output” (rejected because it still looks like a product screen); capture only the generic homepage for API tests (rejected because it is unrelated to their assertions).

## Risks / Trade-offs

- A journey can be mislabeled API-only or browser-driven → enumerate the API-only journey classes in the metadata guard and verify the final grouped report by bounded context.
- Removing screenshots reduces visual material for API-only tests → keep the pass/fail result and trace and label the entry as API/test evidence.
- Mixed UI/API journeys may leave the browser on a stale screen → capture only after the asserted UI flow and ensure the page still represents the tested record.

## Migration Plan

No persisted test data migration is required. New manifests include the evidence mode; historical manifests continue to render with browser mode unless they contain the existing API marker. Retained historical reports remain accessible under their existing bounded-context or Unclassified navigation groups.
