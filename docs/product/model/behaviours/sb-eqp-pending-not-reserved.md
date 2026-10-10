---
id: SB-EQP-PENDING-NOT-RESERVED
type: structured-behaviour
title: "Pending project consumes but does not reserve"
status: draft
illustrates:
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
given:
  - "A project has status pending"
when: "5 lights are planned on the pending project"
then:
  - "Availability of the lights drops by 5 for the project period"
  - "None of the 5 lights is reserved for the project"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/360017701499.md, .private/research/help-center/articles/360017700979.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Distinguishes consuming availability from holding a reservation.

## Boundaries

None.
