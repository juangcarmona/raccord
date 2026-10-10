---
id: FR-INT-001
type: functional-requirement
title: "Authenticate integrations with role-bound tokens"
status: draft
derived-from:
  - "UC-INT-ENABLE-API-ACCESS"
  - "BR-INT-TOKEN-INHERITS-OWNER-ROLE"
  - "BR-INT-TOKEN-VALIDITY-PERIOD"
  - "BR-INT-REGENERATION-REVOKES-TOKEN"
verification:
  - scenario-ref: SB-INT-TOKEN-ROLE-LIMIT
  - scenario-ref: SB-INT-TOKEN-REGENERATED
  - scenario-ref: SB-INT-TOKEN-EXPIRES
uses-terms:
  - "TERM-API-TOKEN"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/30759527977490.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST authenticate every public-interface call with an API token, authorise it with the current role of the token's owner, expire tokens ten years after creation and revoke the previous token on regeneration.

## Rationale

Integrations get stable access without escaping permissions.
