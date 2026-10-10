---
id: JRN-BILL-FINISHED-PROJECT
type: journey
title: "Bill a finished project"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
steps:
  - use-case: "UC-CREATE-PROJECT"
  - use-case: "UC-RECORD-BILLABLE-WORK"
  - use-case: "UC-CREATE-INVOICE-DRAFT"
  - use-case: "UC-REVISE-INVOICE-DRAFT"
  - use-case: "UC-ISSUE-INVOICE"
  - use-case: "UC-OBTAIN-INVOICE-DOCUMENT"
---

## Intended Outcome

Work done on a job becomes a correct, numbered, unalterable invoice with its document, without re-typing.

## Entry Conditions

The issuer is set up and the customer is registered.

## Journey Narrative

The coordinator records the job and its billable work as it happens. When the job ends, the billing administrator selects the unbilled work, reviews the draft and its calculated totals, removes anything that belongs to a later invoice, fixes whatever the draft reports as missing, issues it and downloads the document.

## Variants and Branches

Missing customer details block issuing until completed. Partial billing: unselected lines stay unbilled for a later draft. A discarded draft returns all its work to the project.

## Completion Conditions

The invoice is issued, its document is available and the billed work is marked invoiced.
