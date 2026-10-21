---
id: BR-FIN-MOMENT-TRIGGER-DATES
type: business-rule
title: "Invoice moment parts are due on their trigger"
status: active
applies-to:
  - "UC-FIN-CONFIGURE-INVOICE-MOMENTS"
  - "UC-FIN-WORK-INVOICING-QUEUE"
uses-terms:
  - "TERM-INVOICE-MOMENT"
  - "TERM-INVOICE-MOMENT-TRIGGER"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360017647300.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An invoice moment has one to three parts; a part triggered after agreement is due when the project is confirmed, one triggered before the first day is due before the first day of the planning period, and one triggered afterwards is due after the last day of the planning period.

## Rationale

Matches common payment arrangements such as deposits and post-event billing.

## Examples

See SB-FIN-FIFTY-FIFTY-MOMENTS.

## Exceptions

None.
