---
id: FR-INT-101
type: functional-requirement
title: "Enable tax-authority reporting for a Spanish company"
status: draft
derived-from:
  - "UC-INT-ENABLE-FISCAL-REPORTING"
  - "BR-INT-REPORTING-REQUIRES-AUTHORISATION"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
verification:
  - id: "FISCAL-REPORTING-ENROLMENT"
    scenario: "Given a company registered in Spain, when the administrator uploads the company's digital certificate, reporting is shown as connected and every finalized invoice is reported directly to the tax authority with its status shown on the invoice; without a valid certificate reporting is not connected."
uses-terms:
  - "TERM-COMPLIANCE-RECORD"
provenance:
  source: ".private/research/help-center/articles/27697046471314.md, .private/research/help-center/articles/30759527977490.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let the administrator of a company registered in Spain enable tax reporting by providing the company's digital certificate. The product MUST mark reporting as connected only once a valid certificate is present, MUST then report every finalized invoice and show its reporting status per invoice, and MUST report to the regional system for companies of the Basque region.

## Rationale

Companies subject to anti-fraud invoicing obligations need their invoices reported directly without separate tooling.
