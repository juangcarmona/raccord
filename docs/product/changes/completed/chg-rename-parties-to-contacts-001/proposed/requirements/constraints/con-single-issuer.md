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

An installation serves exactly one issuer; separate companies use separate installations.

## Rationale

Deliberately fixed for the first release to keep isolation simple.

## Consequences

Multi-entity issuing requires a later Product Change.
