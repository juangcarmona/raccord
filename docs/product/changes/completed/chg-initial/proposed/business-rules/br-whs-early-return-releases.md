---
id: BR-WHS-EARLY-RETURN-RELEASES
type: business-rule
title: "Early returns release reservations"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-SCAN-RETURN"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-EARLY-RETURN"
  - "TERM-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment checked in before the end of its planning period is removed from the project's reservation (the planned quantity is unchanged) and becomes reservable by other projects.

## Rationale

Stock back on the shelf should serve the next job.

## Examples

See SB-WHS-EARLY-RETURN-RELEASES.

## Exceptions

None.
