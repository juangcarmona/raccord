---
id: SB-EQP-CANCELLED-RELEASES
type: structured-behaviour
title: "Cancelled project frees equipment"
status: draft
illustrates:
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
given:
  - "A confirmed project has 10 cables planned and reserved"
when: "The project is cancelled"
then:
  - "Its planned equipment no longer reduces availability"
  - "It holds no reservation"
uses-terms:
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/360017701499.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Cancellation releases equipment.

## Boundaries

None.
