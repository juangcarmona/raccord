---
id: SB-WSP-RETURN-EXCESS-BULK
type: structured-behaviour
title: "Scanning more bulk than planned"
status: draft
illustrates:
  - "BR-WSP-SCAN-RETURN-LIMITS"
given:
  - "A project has 10 cables planned"
when: "The technician scans 12 cables against that project"
then:
  - "10 cables are returned to the project"
  - "The product asks how to stock the remaining 2 separately"
uses-terms:
  - "TERM-SCAN-RETURN"
provenance:
  source: ".private/research/help-center/articles/27954393425042.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Planned quantity cap.

## Boundaries

None.
