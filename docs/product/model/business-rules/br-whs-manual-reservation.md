---
id: BR-WHS-MANUAL-RESERVATION
type: business-rule
title: "Planners may reserve manually in any status that consumes availability"
status: draft
applies-to:
  - "UC-WHS-RESERVE-EQUIPMENT"
  - "UC-WHS-REALLOCATE-RESERVATIONS"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-SOFT-HOLD"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360013730520.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A planner may reserve planned equipment manually on a draft or pending project, and may change the reserved quantity of any line in a confirmed project, within the reservable stock of the line's planning period.

## Rationale

Priority is a business decision that can precede confirmation.

## Examples

A pending tender for a key customer reserves its two consoles before the paperwork is signed.

## Exceptions

Inquiry projects are never reserved; cancelled projects cannot be reserved.
