---
id: FR-CRW-ACTIVITY-INVOICE-REFERENCE
type: functional-requirement
title: "Record crew invoice references"
status: draft
derived-from:
  - "UC-CRW-RECORD-ACTIVITY-INVOICE-REFERENCE"
  - "BR-CRW-PLANNED-COST-VISIBILITY"
verification:
  - scenario: "After entering reference INV-77 on three activities, those activities show INV-77 and can be filtered as invoiced"
uses-terms:
  - "TERM-ACTIVITY-INVOICE-REFERENCE"
provenance:
  source: ".private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let users record an invoice reference on selected registered activities and show planned cost only to permitted roles.

## Rationale

Avoid double payment of freelancers.
