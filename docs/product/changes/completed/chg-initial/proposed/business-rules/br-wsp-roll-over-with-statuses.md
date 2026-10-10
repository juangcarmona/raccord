---
id: BR-WSP-ROLL-OVER-WITH-STATUSES
type: business-rule
title: "Roll-overs between projects are tracked with custom warehouse statuses"
status: draft
applies-to:
  - "UC-WHS-CONFIGURE-WAREHOUSE-STATUSES"
uses-terms:
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/35217346228370.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

Equipment moving directly from one project to the next without passing through the warehouse is handled by booking it in the first project into a custom outgoing roll-over status and then returned, pre-booking it in the next project into a custom incoming roll-over status so it is not packed, and setting it on location when it arrives; the product has no dedicated roll-over function.

## Rationale

Back-to-back jobs in peak season make warehouse returns unrealistic.

## Examples

- Two of four microphones from a festival roll over to a corporate event starting the next day.

## Exceptions

None.
