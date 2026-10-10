---
id: SB-WHS-EARLY-RETURN-RELEASES
type: structured-behaviour
title: "Returning early frees stock"
status: draft
illustrates:
  - "BR-WHS-EARLY-RETURN-RELEASES"
  - "UC-WHS-BOOK-MANUALLY"
given:
  - "10 laptops were booked out for a project whose planning period ends Friday"
when: "3 laptops are booked back on Wednesday"
then:
  - "The project's reservation becomes 7"
  - "The planned quantity stays 10"
  - "The 3 laptops are reservable by other projects from Wednesday"
uses-terms:
  - "TERM-EARLY-RETURN"
  - "TERM-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Early returns benefit others.

## Boundaries

The documentation notes that the reservable-stock history may show the change also in the past; that display defect is not a rule.
