---
id: SB-WSP-FOUND-KEEPS-INVOICE
type: structured-behaviour
title: "Found equipment does not credit the customer"
status: draft
illustrates:
  - "BR-WSP-FOUND-NO-FINANCIAL-CORRECTION"
given:
  - "A lost unit was charged to the customer as an additional cost"
when: "The technician marks the unit as found"
then:
  - "Stock increases by one"
  - "The damage charge on the project stays unchanged"
uses-terms:
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/36521619520786.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Warehouse actions do not alter charges.

## Boundaries

None.
