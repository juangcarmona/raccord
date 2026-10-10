---
id: SB-WHS-ALL-AT-STATUS-PROPOSES
type: structured-behaviour
title: "Completing a status proposes a list status change"
status: draft
illustrates:
  - "BR-WHS-STATUS-INDEPENDENCE"
  - "UC-WHS-BOOK-MANUALLY"
given:
  - "All but one item of a list are prepped"
when: "The last item is booked to prepped"
then:
  - "The product proposes to change the list status to prepped"
  - "Declining leaves the list status unchanged"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/13901986722322.md, .private/research/help-center/articles/360015601560.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Status change is suggested, not forced.

## Boundaries

None.
