---
id: BR-CRW-AVAILABILITY-DERIVATION
type: business-rule
title: "Availability is derived from all commitments and declarations"
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

A crew member is unavailable for a period when they are planned on an overlapping function or blocking shift, have an overlapping appointment that blocks other work, declared themselves unavailable, or answered no to an availability invitation; available when they declared availability or answered yes; invited while a request is unanswered; reserved when held by a reservation; otherwise unknown.

## Rationale

Planning decisions need one consolidated signal.

## Examples

A member with an appointment overlapping a function shows unavailable for it.

## Exceptions

None.
