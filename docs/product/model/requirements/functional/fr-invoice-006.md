---
id: FR-INVOICE-006
type: functional-requirement
title: "Issue immutable invoices"
status: draft
derived-from:
  - "UC-ISSUE-INVOICE"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
  - "BR-PARTY-SNAPSHOT"
  - "BR-OPERATION-DATE"
  - "CON-SIF-REGULATION"
verification:
  - scenario: "Issuing a draft produces an invoice with number, issue date, operation date when different, issuer and recipient details, lines, VAT breakdown and total; the draft can no longer be revised, discarded or issued again"
  - scenario: "Any attempt to change or delete an issued invoice, through the product or directly in its records, is refused"
uses-terms:
  - "TERM-ISSUED-INVOICE"
  - "TERM-OPERATION-DATE"
---

## Requirement

The product MUST turn a ready draft into an issued invoice that freezes its content and MUST prevent any change to or deletion of issued invoices.

## Rationale

Integrity and inalterability of invoicing records.
