---
id: BR-CRW-USAGE-VERSUS-PLANNING-PERIOD
type: business-rule
title: "Planning period changes affect cost but not price"
status: draft
applies-to:
  - "UC-CRW-RETIME-FUNCTION"
  - "UC-CRW-RETIME-FUNCTION-GROUP"
  - "UC-CRW-CHANGE-ASSIGNMENT-DETAILS"
uses-terms:
  - "TERM-CREW-PLANNING-PERIOD"
  - "TERM-FUNCTION-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360014594620.md, .private/research/help-center/articles/14232209807506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Changing the crew planning period of an assignment, function or function group changes occupancy and planned cost but never the customer price, which depends only on the function usage period.

## Rationale

Crew may be called earlier than the customer pays for.

## Examples

See SB-CRW-EARLY-CALL-KEEPS-PRICE.

## Exceptions

None.
