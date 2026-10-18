<!-- pdac-scope: none reason="This specification is limited to engineering test evidence and does not define product behavior." -->

## ADDED Requirements

### Requirement: API-only journeys are identified as API evidence

The E2E result writer SHALL record whether each journey's evidence represents browser UI or API/test output. API-only journeys SHALL be explicitly identified as API evidence, omit success and failure product screenshots and browser videos, and retain their verdict and Playwright trace when available. The grouped evidence report SHALL show an API/test evidence explanation and SHALL NOT render screenshot or video assets for an API-only result.

#### Scenario: API-only journey passes

- **WHEN** an API-only journey passes with visual evidence enabled
- **THEN** its result manifest records API evidence mode, its bounded-context group shows the pass verdict and API/test explanation, and no screenshot or video is presented as product UI

#### Scenario: API-only journey fails

- **WHEN** an API-only journey fails
- **THEN** its failed verdict is preserved, an available Playwright trace remains linked, and no screenshot of a test-authored or unrelated page is presented as product evidence

#### Scenario: API result has an accidental visual asset

- **WHEN** an API-mode result is accompanied by a screenshot or video file
- **THEN** the report suppresses that visual asset while preserving the result, its bounded-context placement, and any trace link

### Requirement: UI journey evidence shows the tested application

Journeys that exercise a product UI SHALL capture the actual relevant Raccord screen after the asserted action. Test-authored success HTML SHALL NOT be captured as product evidence. Direct supplemental screenshots, when needed, SHALL be saved under the shared evidence directory and explicitly associated with their owning journey so the existing bounded-context navigation places them correctly.

#### Scenario: Mixed UI and API journey passes

- **WHEN** a journey exercises a real Raccord UI flow and uses API calls for related assertions
- **THEN** it remains browser evidence and its screenshot shows the tested application state rather than a synthetic success page or unrelated homepage

#### Scenario: API-only workflow has no user-facing screen

- **WHEN** an API-only workflow has no corresponding application UI
- **THEN** its report entry is clearly labeled as API/test evidence and is not represented by a fabricated product screenshot
