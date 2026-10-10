---
id: BR-WHS-SPLIT-DETACHES-FROM-VIRTUAL-KIT
type: business-rule
title: "Splitting kit content takes it out of the kit"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A line planned with quantity two or more may be split into separate booking lines with chosen quantities; splitting content of a virtual combination places both resulting lines outside the combination.

## Rationale

Allows partial handling without corrupting kit structure.

## Examples

Splitting 10 cables into 6 and 4 for two trucks.

## Exceptions

None.
