---
id: SB-WHS-SCAN-RETURN-BULK
type: structured-behaviour
title: "Scan return needs project and quantity"
status: draft
illustrates:
  - "BR-WHS-SCAN-RETURN-ROUTING"
  - "UC-WHS-SCAN-RETURN"
given:
  - "Cables are out on two projects"
when: "The technician scans the cable code in scan return"
then:
  - "The product asks which project and how many are returned"
uses-terms:
  - "TERM-SCAN-RETURN"
provenance:
  source: ".private/research/help-center/articles/13959262021394.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Pieces of an article are interchangeable.

## Boundaries

None.
