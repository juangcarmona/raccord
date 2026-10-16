---
id: CON-SINGLE-ISSUER
type: constraint
title: "One issuing organization per installation"
status: draft
applies-to:
  - "BC-CONTACTS"
  - "BC-INVOICING"
uses-terms:
  - "TERM-ISSUER"
---

## Constraint

An installation serves exactly one issuing legal entity. Address, contact-detail or legal-identity corrections for that same entity do not create a second issuer and must preserve issued-invoice snapshots. A change that represents a different legal entity requires a separate installation; one installation must not switch between issuing companies.

## Rationale

Separate companies need independent issuer identities and invoice records, while a correction to the same issuer must not change previously issued invoices.

## Consequences

Multi-entity issuing within one installation requires a later Product Change. A new installation is a product boundary for a different issuer, not a conclusion that every tax-identifier update requires one.
