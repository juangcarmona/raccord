---
id: SB-FIN-QUEUE-OVERDUE-HIGHLIGHT
type: structured-behaviour
title: "Overdue invoice moment is highlighted"
status: draft
illustrates:
  - "BR-FIN-QUEUE-ELIGIBILITY"
given:
  - "A confirmed project has an after-the-event moment due 3 days ago"
  - "No invoice has been generated"
when: "The administrator views the invoicing queue for this month"
then:
  - "The project is listed"
  - "It is highlighted as overdue"
uses-terms:
  - "TERM-INVOICING-QUEUE"
  - "TERM-QUEUE-ENTRY-STATUS"
provenance:
  source: ".private/research/help-center/articles/360017700359.md, .private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Missed billing is visible.

## Boundaries

None.
