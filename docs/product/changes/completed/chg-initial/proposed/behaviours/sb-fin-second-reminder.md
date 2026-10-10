---
id: SB-FIN-SECOND-REMINDER
type: structured-behaviour
title: "Second reminder extends the deadline"
status: draft
illustrates:
  - "BR-FIN-REMINDERS-MANUAL"
given:
  - "An unpaid invoice is overdue and has one reminder"
when: "The administrator generates a second reminder with due date 30 May"
then:
  - "The invoice shows 2 reminders sent"
  - "The last reminder date is today"
  - "The invoice is due 30 May"
uses-terms:
  - "TERM-PAYMENT-REMINDER"
provenance:
  source: ".private/research/help-center/articles/17195865170706.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Reminder tracking.

## Boundaries

Nothing is sent automatically.
