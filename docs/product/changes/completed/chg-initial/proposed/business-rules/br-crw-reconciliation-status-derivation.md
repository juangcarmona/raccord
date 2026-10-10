---
id: BR-CRW-RECONCILIATION-STATUS-DERIVATION
type: business-rule
title: "Reconciliation status compares registered with planned time"
status: draft
applies-to:
  - "UC-CRW-RECONCILE-PLANNED-AND-REGISTERED-TIME"
uses-terms:
  - "TERM-RECONCILIATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/23814779870610.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

An activity in the future is planned; a past activity without registration is no registration; otherwise it is exceeds time, aligned or under time when registered time is longer than, equal to or shorter than planned time.

## Rationale

Shared vocabulary for payroll and budget review.

## Examples

Planned 8, registered 10: exceeds time with difference 2 hours.

## Exceptions

None.
