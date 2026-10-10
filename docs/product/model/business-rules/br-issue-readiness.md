---
id: BR-ISSUE-READINESS
type: business-rule
title: "Conditions for issuing an invoice"
status: draft
applies-to:
  - "UC-ISSUE-INVOICE"
uses-terms:
  - "TERM-VALIDATION-ISSUE"
  - "TERM-ISSUER"
  - "TERM-CUSTOMER"
---

## Rule

A draft can be issued only if it has at least one line, its total is greater than zero, the issuer has legal name, tax identifier and complete address, the recipient has legal name, tax identifier and complete address, and the issue date is accepted by the series; every unmet condition is reported at once.

## Rationale

Spanish full invoices must identify both parties; reporting every issue at once saves round trips.

## Examples

See SB-MISSING-RECIPIENT-TAX-ID.

## Exceptions

A rectifying invoice may have a negative total (BR-RECTIFYING-INVOICE).
