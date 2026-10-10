---
id: BR-WHS-CHARGE-TO-PROJECT
type: business-rule
title: "Charges for loss become project costs"
status: draft
applies-to:
  - "UC-WHS-MARK-LOST"
uses-terms:
  - "TERM-DAMAGE-CHARGE"
  - "TERM-ADDITIONAL-COST"
provenance:
  source: ".private/research/help-center/articles/17531415206418.md, .private/research/help-center/articles/360014392200.md, .private/research/help-center/articles/13901986840082.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a loss is charged to the customer, its name and amount are added to the project as an additional cost, and for losses the amount is shown as the lost item's sale value; when not charged no cost is added.

## Rationale

Recovering losses must reach billing.

## Examples

See SB-WHS-LOSS-CHARGED.

## Exceptions

None.
