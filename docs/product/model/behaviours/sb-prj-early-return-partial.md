---
id: SB-PRJ-EARLY-RETURN-PARTIAL
type: structured-behaviour
title: "Items returned early while the project stays on location"
status: draft
illustrates:
  - "BR-WHS-STATUS-INDEPENDENCE"
  - "BR-INT-PROJECT-MANAGEMENT-LIFECYCLE"
given:
  - "A project is In progress with 10 rented items on location"
when: "5 items are booked back as returned"
then:
  - "Those 5 items show the returned warehouse status"
  - "The project status remains In progress"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Project and item statuses evolve independently; partial returns do not end execution.

## Boundaries

None.
