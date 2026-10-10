---
id: FR-INVOICE-001
type: functional-requirement
title: "Create invoice drafts from unbilled work"
status: draft
derived-from:
  - "UC-CREATE-INVOICE-DRAFT"
  - "BR-SINGLE-BILLING"
  - "BR-LINE-ORDER"
verification:
  - scenario-ref: "SB-LINE-IN-TWO-DRAFTS"
  - scenario-ref: "SB-DRAFT-FOLLOWS-PROJECT-ORDER"
uses-terms:
  - "TERM-INVOICE-DRAFT"
---

## Requirement

The product MUST create a draft from selected unbilled lines of one project, reserving them, and MUST refuse lines that are already in a draft or invoiced.

## Rationale

Drafts are the only path from work to invoices.
