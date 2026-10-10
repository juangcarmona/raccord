---
id: BR-CRW-PARTIAL-AVAILABILITY
type: business-rule
title: "Partial unavailability makes the whole period unavailable"
status: draft
applies-to:
  - "UC-CRW-VIEW-CREW-AVAILABILITY"
uses-terms:
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/360014685859.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

If a crew member is unavailable for any part of a function's period, they are unavailable for the function; if they are available for part and unknown for the rest, they count as available.

## Rationale

Unknown is not a refusal, but any known clash is.

## Examples

See SB-CRW-PARTIAL-UNAVAILABLE and SB-CRW-PARTIAL-AVAILABLE-UNKNOWN.

## Exceptions

None.
