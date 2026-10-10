---
id: SB-PRJ-DRAFT-NOT-IN-WAREHOUSE
type: structured-behaviour
title: "Unconfirmed projects are not handled in the warehouse"
status: draft
illustrates:
  - "BR-PRJ-STATUS-AREA-VISIBILITY"
given:
  - "A project with planned equipment is Pending"
when: "A technician looks for it in warehouse handling"
then:
  - "The project is not listed"
  - "It cannot be invoiced from the invoicing area either"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Confirmation gates physical handling and invoicing.

## Boundaries

None.
