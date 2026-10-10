---
id: BR-CRW-HOUR-TYPE-TOTALS
type: business-rule
title: "Each hour type has a fixed effect on worked totals"
status: draft
applies-to:
  - "UC-CRW-REGISTER-HOURS-MANUALLY"
  - "UC-CRW-EXPORT-PAYROLL-DATA"
uses-terms:
  - "TERM-HOUR-TYPE"
provenance:
  source: ".private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Worked hours add to the worked total; correction adds or subtracts; compensation does not add.

## Rationale

Payroll needs to separate kinds of time.

## Examples

Compensation hours registered on Friday leave the worked total unchanged.

## Exceptions

How a custom hour type counts is chosen when the type is configured.
