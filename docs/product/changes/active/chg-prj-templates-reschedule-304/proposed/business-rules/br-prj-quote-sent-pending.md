---
id: BR-PRJ-QUOTE-SENT-PENDING
type: business-rule
title: "Sending a quotation makes the project pending"
status: active
applies-to:
  - "UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY"
  - "UC-FIN-SEND-QUOTE"
uses-terms:
  - "TERM-PROJECT-STATUS-PENDING"
  - "TERM-QUOTE-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a quotation of a project is sent to the customer, the project's status changes automatically to Pending.

## Rationale

The project now awaits the customer.

## Examples

See SB-PRJ-QUOTE-SENT-PENDING.

## Exceptions

Status may be set manually afterwards.
