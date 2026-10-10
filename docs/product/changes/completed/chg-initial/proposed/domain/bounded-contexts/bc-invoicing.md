---
id: BC-INVOICING
type: bounded-context
title: "Invoicing"
status: draft
---

## Responsibility

Preparing, validating, numbering and issuing invoices, and keeping issued invoices unaltered and reproducible.

## Language

Invoices and their drafts, invoice moments, deductions, credit and rectifying invoices, series and numbers, issue and operation dates, VAT breakdowns, payments, reminders, payment terms, deposits, ledgers, exports and e-invoices.

## Boundaries

Quotes, contracts, pricing and discounts belong to Commercial; document layout to Documents; tax reporting to Compliance and audit.

## External Relationships

Consumes billable lines from Projects and party snapshots from Parties; hands every issued invoice to Compliance.
