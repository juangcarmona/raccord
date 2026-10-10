---
id: BR-WHS-PHYSICAL-KIT-SHARED-TIME
type: business-rule
title: "Content of a physical combination shares its time"
status: draft
applies-to:
  - "UC-WHS-SET-LINE-TIMES"
uses-terms:
  - "TERM-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Content lines of a physical combination always carry the same periods as the combination; individual content of a virtual combination may carry its own periods.

## Rationale

A physical case travels as one unit.

## Examples

A rack's amplifiers cannot leave a day later than the rack.

## Exceptions

None.
