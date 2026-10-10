---
id: FR-FIN-INVOICING-QUEUE
type: functional-requirement
title: "Maintain the invoicing queue"
status: draft
derived-from:
  - "UC-FIN-WORK-INVOICING-QUEUE"
  - "UC-FIN-MARK-INVOICED-EXTERNALLY"
  - "BR-FIN-QUEUE-ELIGIBILITY"
  - "BR-FIN-EXTERNAL-INVOICING"
verification:
  - scenario-ref: "SB-FIN-QUEUE-OVERDUE-HIGHLIGHT"
  - scenario: "A project marked as invoiced externally leaves the invoicing queue and appears in the externally invoiced list."
uses-terms:
  - "TERM-INVOICING-QUEUE"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360017700359.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST list confirmed projects with invoice moment parts due in a period, highlight overdue ones and allow marking them invoiced externally.

## Rationale

Prevents unbilled work.
