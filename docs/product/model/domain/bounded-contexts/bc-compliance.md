---
id: BC-COMPLIANCE
type: bounded-context
title: "Compliance and audit"
status: draft
---

## Responsibility

Recording who did what, and the obligations issued invoices create towards the tax authority.

## Language

Audit events, compliance records and billing records.

## Boundaries

Interpreting tax law is outside the product; requirements come from official sources and legal review.

## External Relationships

Receives every issued invoice from Invoicing; communicates with the tax authority.
