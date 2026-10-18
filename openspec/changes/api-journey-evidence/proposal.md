<!-- pdac-scope: none reason="This change corrects engineering test evidence and does not change product behavior." -->

## Why

Several selected API-driven journeys replace the browser page with test-authored success HTML and capture it as if it showed Raccord. Other API-only journeys capture the generic signed-in homepage, which is unrelated to their assertions. The current report already has an API evidence note, but the journey runner does not write an API evidence mode, so the note is never activated.

## What Changes

- Classify API-only journeys explicitly and persist that classification in their result manifests.
- Suppress product screenshots and browser videos for API-only journeys while retaining their test verdict and Playwright trace.
- Render API-only results as API/test evidence under their existing bounded-context groups, and ignore any stray screenshot or video attached to an API result.
- Remove test-authored success pages and unrelated direct screenshots from the selected API-only journeys; retain real application screenshots for mixed UI/API journeys.
- Keep generated evidence synthetic and associate every retained supplemental asset with its owning journey and context.

## Capabilities

### New Capabilities

- `api-journey-evidence`: report API-only journeys as test/API evidence without presenting synthetic pages or unrelated product UI as screenshots.

### Modified Capabilities

- None.

## Impact

The shared E2E journey runner and result writer, API-only journey traits, static evidence report builder and tests, plus selected per-PR E2E results across Contacts, Projects, Equipment, Warehouse, Crew and transport, Invoicing, Compliance and audit, Documents and communication, and Workspace. Browser journeys keep their current screenshot/video behavior. No application behavior, production dependency, product scope, or evidence publication audience changes.
