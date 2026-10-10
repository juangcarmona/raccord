---
id: SB-WHS-UNPLANNED-SCAN
type: structured-behaviour
title: "Scanning an unplanned item"
status: draft
illustrates:
  - "BR-WHS-UNPLANNED-SCAN-ADDS"
  - "UC-WHS-BOOK-BY-SCANNING"
given:
  - "A list does not plan a fog machine"
when: "The technician scans a fog machine's code"
then:
  - "The product asks whether to add it as additional equipment"
  - "On confirmation it is added in a new equipment group and booked"
uses-terms:
  - "TERM-ADDITIONAL-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015601560.md, .private/research/help-center/articles/13901986840082.md"
  confidence: high
  recovered-from: documentation
---

## Intent

No silent additions.

## Boundaries

None.
