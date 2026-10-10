---
id: FR-INT-104
type: functional-requirement
title: "Replace a possibly exposed API token"
status: draft
derived-from:
  - "UC-INT-REGENERATE-API-TOKEN"
  - "BR-INT-REGENERATION-REVOKES-TOKEN"
verification:
  - scenario-ref: "SB-INT-TOKEN-REGENERATED"
uses-terms:
  - "TERM-API-TOKEN"
  - "TERM-API-TOKEN-VALIDITY"
provenance:
  source: ".private/research/help-center/articles/360013767839.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let an administrator with public-interface access request a new API token, MUST create the new token and revoke the previous one immediately so that only the new token authenticates, and MUST refuse applications that still use the old token until they are updated.

## Rationale

A shared or expired token must be replaceable at once to protect the workspace.
