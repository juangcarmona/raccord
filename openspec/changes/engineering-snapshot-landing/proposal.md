<!-- pdac-scope: none reason="Improves the public engineering snapshot portal and its CI evidence presentation; it does not change product behavior or semantics." -->

# Engineering snapshot landing page

Make the public engineering snapshot landing page readable and useful across time zones, browser themes and E2E result states.

## Why

The landing page currently exposes an ISO UTC timestamp without local context and reports workflow success without showing whether browser journeys were actually recorded. Its appearance also differs from the architecture and product-model snapshots.

## What changes

- Format the latest-run time using the visitor's browser locale and time zone, retaining a machine-readable timestamp and UTC fallback.
- Show the tested revision and journey totals, and explicitly identify a run with no recorded journeys even if its workflow status was successful.
- Align landing-page light, dark and system/automatic appearance controls with the other snapshots and persist the shared preference.
- Keep the three snapshot destinations clear and responsive, with accessible control states, focus indicators and reduced-motion support.

## Capabilities

### New capabilities

- `engineering-snapshot-portal`: present navigation and the latest browser evidence consistently and accurately.

## Impact

Only the static-site generator, its tests, and engineering-site change documentation are affected. No product behavior, APIs, runtime deployment or production dependency changes.
