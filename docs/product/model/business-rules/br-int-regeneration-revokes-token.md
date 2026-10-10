---
id: BR-INT-REGENERATION-REVOKES-TOKEN
type: business-rule
title: "Regenerating revokes the previous token"
status: draft
applies-to:
  - "UC-INT-REGENERATE-API-TOKEN"
uses-terms:
  - "TERM-API-TOKEN"
provenance:
  source: ".private/research/help-center/articles/360013767839.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Creating a new token for the workspace's interface access immediately invalidates the previous one.

## Rationale

Regeneration is the way to cut off a leaked token.

## Examples

After regeneration a script still using the old token receives authentication refusals.

## Exceptions

None.
