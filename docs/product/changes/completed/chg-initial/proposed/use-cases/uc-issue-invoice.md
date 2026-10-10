---
id: UC-ISSUE-INVOICE
type: use-case
title: "Issue an invoice"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-TAX-AUTHORITY"
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-ISSUE-READINESS"
  - "BR-GAPLESS-NUMBERING"
  - "BR-ISSUE-DATE-ORDER"
  - "BR-OPERATION-DATE"
  - "BR-PARTY-SNAPSHOT"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
  - "BR-INTEGRITY-VERIFICATION"
  - "BR-COMPLIANCE-OBLIGATION"
  - "BR-AUDITED-ACTIONS"
  - "BR-VAT-PER-RATE"
uses-terms:
  - "TERM-ISSUED-INVOICE"
  - "TERM-INVOICE-NUMBER"
  - "TERM-INVOICE-FINGERPRINT"
---

## Goal

A valid draft becomes a numbered, immutable invoice with its document, fingerprint, audit trail and compliance record.

## Trigger

The administrator decides to invoice the draft.

## Preconditions

The draft is open.

## Main Flow

1. The administrator confirms the issue date (default: today in the issuer's time zone).
2. The product checks issue readiness.
3. The product allocates the next number of the series and year, freezes parties, lines and totals, and calculates the fingerprint.
4. The billed lines become invoiced.
5. The product produces and stores the invoice document, creates the compliance record and audits the issue and the document, all together.
6. The administrator sees the issued invoice.

## Alternative Flows

Another invoice of the same series is being issued at the same moment: one of them is asked to retry; numbers stay gapless.

## Failure Conditions

Unmet readiness conditions are all reported and nothing is issued; no number is consumed. Issuing an already issued draft is refused.

## Postconditions

The invoice is issued, its lines are invoiced, and its compliance record is pending.
