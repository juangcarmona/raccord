---
id: "SB-TRB-ROLE-CHANGE-REVEALS-MODULES"
type: structured-behaviour
title: "New role shows new modules"
status: draft
illustrates:
  - "BR-WSP-HIDDEN-MODULES"
  - "UC-WSP-ASSIGN-ROLE"
given:
  - "John holds the Warehouse role without crew planning or invoicing"
when: "An administrator changes John's role to the Office role that grants crew planning and invoicing"
then:
  - "John sees the crew planning module"
  - "John can send invoices to customers"
uses-terms:
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/360014521820.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Missing modules are usually a role matter.

## Boundaries

When the change takes effect in an open session is described elsewhere (session renewal).
