---
id: BR-PRJ-CANCEL-RELEASES-EQUIPMENT
type: business-rule
title: "Cancelling releases equipment reservations"
status: active
applies-to:
  - "UC-PRJ-CANCEL-PROJECT"
uses-terms:
  - "TERM-PROJECT-STATUS-CANCELLED"
provenance:
  source: ".private/research/help-center/articles/28083930405778.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a project is cancelled, its equipment reservations are removed immediately and the items become available to other projects.

## Rationale

Cancelled work must not block stock.

## Examples

See SB-PRJ-CANCEL-FREES-STOCK.

## Exceptions

None.
