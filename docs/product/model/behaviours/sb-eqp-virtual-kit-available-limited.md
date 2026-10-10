---
id: SB-EQP-VIRTUAL-KIT-AVAILABLE-LIMITED
type: structured-behaviour
title: "Virtual kit availability limited by available content"
status: draft
illustrates:
  - "BR-EQP-VIRTUAL-KIT-STOCK-DERIVED"
given:
  - "A virtual kit needs 1 of item A, 3 of item B and 4 of item C"
  - "Over a period, 2 of A, 12 of B and 100 of C are available and unreserved"
when: "Availability of the virtual kit is computed for that period"
then:
  - "2 kits are available"
  - "Item A is the limiting content"
uses-terms:
  - "TERM-VIRTUAL-KIT"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows that availability, not just stock, is derived from content.

## Boundaries

None.
