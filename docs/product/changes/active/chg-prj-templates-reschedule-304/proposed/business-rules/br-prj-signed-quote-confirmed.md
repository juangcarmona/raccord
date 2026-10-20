---
id: BR-PRJ-SIGNED-QUOTE-CONFIRMED
type: business-rule
title: "A digitally signed quotation confirms the project"
status: active
applies-to:
  - "UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY"
  - "UC-FIN-RESPOND-TO-ONLINE-QUOTE"
uses-terms:
  - "TERM-PROJECT-STATUS-CONFIRMED"
  - "TERM-SIGNED-QUOTE"
  - "TERM-QUOTE-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When the customer digitally signs a quotation of a project, the project's status changes automatically to Confirmed.

## Rationale

Signature is the customer's agreement.

## Examples

See SB-PRJ-SIGNED-QUOTE-CONFIRMS.

## Exceptions

Confirmations received by other means are set manually.
