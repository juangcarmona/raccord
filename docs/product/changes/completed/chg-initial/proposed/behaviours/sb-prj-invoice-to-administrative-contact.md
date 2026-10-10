---
id: SB-PRJ-INVOICE-TO-ADMINISTRATIVE-CONTACT
type: structured-behaviour
title: "Invoice recipient selection"
status: draft
illustrates:
  - "BR-PRJ-INVOICE-RECIPIENT"
given:
  - "Contact Acme has default contact person Jo and administrative contact person finance desk"
when: "An invoice for Acme is sent"
then:
  - "The finance desk is preselected as recipient"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Invoices go to finance.

## Boundaries

None.
