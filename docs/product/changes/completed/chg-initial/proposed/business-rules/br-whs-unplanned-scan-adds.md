---
id: BR-WHS-UNPLANNED-SCAN-ADDS
type: business-rule
title: "Scanning unplanned equipment proposes it as additional"
status: draft
applies-to:
  - "UC-WHS-BOOK-BY-SCANNING"
  - "UC-WHS-ADD-ADDITIONAL-EQUIPMENT"
uses-terms:
  - "TERM-ADDITIONAL-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015601560.md, .private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/12692565544594.md, .private/research/help-center/articles/36515689863186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Scanning a code not planned on the packing list does not book it silently: the product asks to add it as additional equipment, which then goes into a new equipment group of the project.

## Rationale

Nothing should leave unrecorded or by mistake.

## Examples

See SB-WHS-UNPLANNED-SCAN.

## Exceptions

None.
