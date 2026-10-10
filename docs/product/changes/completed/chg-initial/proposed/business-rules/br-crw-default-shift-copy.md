---
id: BR-CRW-DEFAULT-SHIFT-COPY
type: business-rule
title: "Shifts copy default shift values and stay independent"
status: draft
applies-to:
  - "UC-CRW-CREATE-SHIFT"
  - "UC-CRW-MANAGE-DEFAULT-SHIFTS"
uses-terms:
  - "TERM-DEFAULT-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A shift created from a default shift takes over its values; changing that shift afterwards does not change the default shift.

## Rationale

Exceptions without altering the template.

## Examples

Default quantity two, one shift set to three; the default stays two.

## Exceptions

None.
