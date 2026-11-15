---
id: UC-REPORT-BILLING-RECORDS
type: use-case
title: "Report billing records to the tax authority"
status: draft
primary-actor: "ACT-TAX-AUTHORITY"
supporting-actors:
  - "ACT-BILLING-ADMINISTRATOR"
bounded-context: "BC-COMPLIANCE"
governed-by:
  - "BR-COMPLIANCE-OBLIGATION"
  - "BR-VERIFACTU-DIRECT-SUBMISSION"
uses-terms:
  - "TERM-COMPLIANCE-RECORD"
---

## Goal

Every issued invoice is reported as a chained billing record and its outcome is known.

## Trigger

A compliance record is pending.

## Preconditions

Reporting is enabled and a valid company electronic certificate is installed.

## Main Flow

1. The product builds the billing record of each pending compliance record, in issue order.
2. The product sends it to the tax authority and keeps the evidence of the response.
3. The tax authority accepts, accepts with errors or rejects it.

## Alternative Flows

The tax authority is unreachable: records stay pending and are retried with the same content.

## Failure Conditions

Rejections surface to the billing administrator as corrections to make; issued invoices never change.

## Postconditions

Each compliance record shows its reporting outcome.
