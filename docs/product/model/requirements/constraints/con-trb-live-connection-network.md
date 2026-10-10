---
id: "CON-TRB-LIVE-CONNECTION-NETWORK"
type: constraint
title: "Live updates need an unblocked persistent connection"
status: draft
applies-to:
  - "UC-TRB-SEE-LIVE-CHANGES"
uses-terms:
  - "TERM-LIVE-UPDATE-CONNECTION"
provenance:
  source: ".private/research/help-center/articles/360013988420.md, .private/research/help-center/articles/360016035999.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

Live updates and presence use a persistent web-socket connection from the browser to the product's live-update endpoint; it must not be blocked by ad or cookie blockers, antivirus or firewall software, proxies or VPNs.

## Rationale

Imposed by the network path between the user's browser and the product, which the product does not control.

## Consequences

Company IT must allow the live-update endpoint; when it is blocked users see recurring connection-lost warnings and may work with stale data.
