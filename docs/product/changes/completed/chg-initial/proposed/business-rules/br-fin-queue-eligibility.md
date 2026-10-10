---
id: BR-FIN-QUEUE-ELIGIBILITY
type: business-rule
title: "When a project enters the invoicing queue"
status: draft
applies-to:
  - "UC-FIN-WORK-INVOICING-QUEUE"
uses-terms:
  - "TERM-INVOICING-QUEUE"
  - "TERM-QUEUE-ENTRY-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017700359.md, .private/research/help-center/articles/360013628279.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A project appears in the invoicing queue when it is confirmed and the trigger date of one of its invoice moment parts falls in the selected period and that part is neither invoiced nor marked invoiced externally; parts whose date has passed are highlighted.

## Rationale

Nothing confirmed should go unbilled.

## Examples

See SB-FIN-QUEUE-OVERDUE-HIGHLIGHT.

## Exceptions

Unconfirmed projects never appear.
