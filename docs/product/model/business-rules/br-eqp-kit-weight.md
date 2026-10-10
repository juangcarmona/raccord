---
id: BR-EQP-KIT-WEIGHT
type: business-rule
title: "Kit weight and volume"
status: draft
applies-to:
  - "UC-EQP-DEFINE-PHYSICAL-KIT"
  - "UC-EQP-DEFINE-VIRTUAL-KIT"
  - "UC-EQP-CONFIGURE-ITEM-DETAILS"
uses-terms:
  - "TERM-EMPTY-WEIGHT"
  - "TERM-TRANSPORT-VOLUME"
  - "TERM-PHYSICAL-KIT"
  - "TERM-VIRTUAL-KIT"
  - "TERM-ACCESSORY"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md, .private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A physical kit weighs its empty weight plus the weight of its default content and occupies the volume given by its own dimensions; a virtual kit has no weight or volume of its own and its content's weight and volume are used; accessories count individually.

## Rationale

Transport totals must match what is loaded.

## Examples

- A case of 12 kg empty with content of 18 kg weighs 30 kg.

## Exceptions

None.
