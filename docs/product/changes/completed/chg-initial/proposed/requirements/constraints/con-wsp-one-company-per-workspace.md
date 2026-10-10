---
id: CON-WSP-ONE-COMPANY-PER-WORKSPACE
type: constraint
title: "An installation serves one company"
status: draft
applies-to:
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-WORKSPACE"
provenance:
  source: ".private/research/help-center/articles/36522581714322.md, .private/research/help-center/articles/36812030570770.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

An installation carries one set of shared configuration for one company: project types, templates, warehouse statuses, custom fields and financial settings are common to all users; the mail account used for sending, terms and conditions and the logo on the signing page exist once. Equipment is visible to all users, the same item cannot carry different prices, factors, discounts or ledger codes per entity, and access cannot be restricted per entity.

## Rationale

Deliberate product boundary: the product is installed and configured by one company for itself.

## Consequences

- Several legal entities using one installation can only be told apart by naming, project types, tags or custom fields.
- Entities needing separate financial administration or data separation need their own installation.
