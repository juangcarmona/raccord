---
id: BR-CRW-PAYROLL-CODE-IN-EXPORT
type: business-rule
title: "Exports carry the payroll code of each hour type"
status: draft
applies-to:
  - "UC-CRW-EXPORT-PAYROLL-DATA"
  - "UC-CRW-CONFIGURE-HOUR-TYPES"
uses-terms:
  - "TERM-PAYROLL-CODE"
provenance:
  source: ".private/research/help-center/articles/24569529468562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When an hour type has a payroll code, exported hours of that type carry it.

## Rationale

Smooth import into payroll systems.

## Examples

Overtime hours exported with code OT01.

## Exceptions

None.
