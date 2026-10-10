---
id: "CON-TRB-SUPPORTED-BROWSERS"
type: constraint
title: "Current versions of mainstream browsers are supported"
status: draft
applies-to:
  - "UC-WSP-SIGN-IN"
  - "UC-TRB-SEE-LIVE-CHANGES"
uses-terms:
  - "TERM-WORKSPACE"
provenance:
  source: ".private/research/help-center/articles/360016035999.md, .private/research/help-center/articles/360013988420.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

The web application, including the installable mobile web app, supports the latest versions of the mainstream desktop and mobile browsers (Chromium-based browsers, Safari and Firefox); outdated browsers, and the discontinued legacy Windows browser in particular, are not supported. JavaScript must be enabled.

## Rationale

Deliberate support boundary that keeps the product secure and maintainable.

## Consequences

Users on old browser versions may encounter errors; the first troubleshooting step is updating the browser or trying a mainstream one.
