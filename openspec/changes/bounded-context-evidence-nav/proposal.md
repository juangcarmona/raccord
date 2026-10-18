<!-- pdac-scope: none reason="E2E test classifications and engineering evidence rendering do not change product behavior." -->

## Why

The nightly browser report has a flat journey list, so reviewers cannot find evidence through the product's bounded contexts. The closed issue audit also surfaced tests and capabilities with different readiness; the evidence report needs to show which contexts have coverage and which have none.

## What Changes

- Record bounded-context labels alongside each Playwright journey result.
- Group the evidence site's left navigation by the ten product bounded contexts, with explicit empty and cross-cutting groups.
- Keep existing evidence assets, report history, and old manifests readable.
- Assign the existing customer-registration journey to Contacts and keep the per-PR E2E battery intact.

## Capabilities

### New Capabilities

None. This change only classifies test results and changes the engineering evidence presentation.

### Modified Capabilities

None.

## Impact

`tests/Raccord.EndToEndTests/Infrastructure/DistributedAppFixture.cs`, `tests/Raccord.EndToEndTests/Infrastructure/PlaywrightArtifacts.cs`, concrete E2E journey traits, `tests/Raccord.EndToEndTests/per-pr-selection.txt`, `src/Raccord.Aspire/Program.cs`, `tools/static-site/static-site.mjs`, and its Node tests. AppHost keeps persistent Postgres storage for local development; the E2E builder detaches that mount for clean per-run storage. The AppHost Vite path points to the frontend project location. No application API, production dependency, or deployment audience changes.
