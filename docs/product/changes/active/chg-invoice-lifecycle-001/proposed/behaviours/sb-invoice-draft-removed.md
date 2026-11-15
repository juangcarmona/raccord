---
id: SB-INVOICE-DRAFT-REMOVED
type: structured-behaviour
title: "Discarding a draft retains its record"
status: draft
illustrates:
  - "UC-REVISE-INVOICE-DRAFT"
  - "BR-SINGLE-BILLING"
  - "BR-AUDITED-ACTIONS"
given:
  - "An open draft contains a billable line"
when: "The billing administrator discards the draft"
then:
  - "The draft has Removed status and remains queryable"
  - "Its billable line is unbilled and available for another draft"
  - "The discard is recorded in the audit trail"
  - "The removed draft cannot be revised or issued"
uses-terms:
  - "TERM-INVOICE-STATUS"
  - "TERM-INVOICE-DRAFT"
  - "TERM-BILLABLE-LINE"
covers-transition:
  lifecycle: "LC-INVOICE-STATUS"
  transition: DISCARD
---

## Intent

Show the observable result of discarding a draft without losing its history.

## Boundaries

Does not set the retention duration or the query interface.
