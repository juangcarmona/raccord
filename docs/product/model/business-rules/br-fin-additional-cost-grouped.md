---
id: BR-FIN-ADDITIONAL-COST-GROUPED
type: business-rule
title: "Every additional cost belongs to a cost group"
status: draft
applies-to:
  - "UC-FIN-MANAGE-ADDITIONAL-COSTS"
uses-terms:
  - "TERM-COST-GROUP"
  - "TERM-ADDITIONAL-COST"
provenance:
  source: ".private/research/help-center/articles/360013881760.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Every additional cost belongs to exactly one cost group; adding a cost when no group exists creates a default group, and when groups exist the user must choose one; each group shows a subtotal.

## Rationale

Documents present costs in clear sections.

## Examples

The first hotel cost on a project creates a default group.

## Exceptions

None.
