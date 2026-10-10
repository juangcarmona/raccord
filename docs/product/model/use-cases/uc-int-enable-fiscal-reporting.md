---
id: UC-INT-ENABLE-FISCAL-REPORTING
type: use-case
title: "Enable tax-authority reporting"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-TAX-AUTHORITY"
bounded-context: "BC-COMPLIANCE"
governed-by:
  - "BR-INT-REPORTING-REQUIRES-AUTHORISATION"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
uses-terms:
  - "TERM-COMPLIANCE-RECORD"
provenance:
  source: ".private/research/help-center/articles/27697046471314.md, .private/research/help-center/articles/30759527977490.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Finalized invoices of a Spanish company are reported directly to the national or regional tax system.

## Trigger

The company must comply with the anti-fraud invoicing-software obligations.

## Preconditions

The company is registered in Spain and holds a digital certificate.

## Main Flow

1. The administrator opens the tax reporting settings.
2. The administrator confirms the company's tax details and uploads the company's digital certificate.
3. The product marks reporting as connected; from then on every finalized invoice is reported and its reporting status is shown per invoice.

## Alternative Flows

Basque-region companies report to the regional system instead of the national one.

## Failure Conditions

Certificate missing, invalid or expired: reporting is not connected.

## Postconditions

Reporting is connected; finalized invoices are immutable and reported (UC-REPORT-BILLING-RECORDS).
