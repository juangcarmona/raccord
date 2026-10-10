---
id: BR-CRW-APPROVED-READ-ONLY
type: business-rule
title: "Approved registrations cannot be changed until reverted"
status: draft
applies-to:
  - "UC-CRW-APPROVE-TIME-REGISTRATIONS"
  - "UC-CRW-REVERT-TIME-REGISTRATION-APPROVAL"
uses-terms:
  - "TERM-TIME-REGISTRATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

An approved time registration is read-only; it can be changed only after it is reverted to pending, and must then be approved again.

## Rationale

Approved hours are a stable basis for payroll.

## Examples

See SB-CRW-REVERT-THEN-EDIT.

## Exceptions

None.
