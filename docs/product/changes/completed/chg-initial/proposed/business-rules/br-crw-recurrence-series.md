---
id: BR-CRW-RECURRENCE-SERIES
type: business-rule
title: "Recurring items are created as a series up to an end date"
status: draft
applies-to:
  - "UC-CRW-ADD-APPOINTMENT"
  - "UC-CRW-ENTER-CREW-AVAILABILITY"
  - "UC-CRW-CREATE-SHIFT"
uses-terms:
  - "TERM-RECURRENCE-PATTERN"
provenance:
  source: ".private/research/help-center/articles/23917302179218.md, .private/research/help-center/articles/17290277763090.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

An item with a recurrence pattern creates one occurrence per matching date from the start until the end date; a recurring shift without an end date creates only one shift.

## Rationale

Repetition must be bounded.

## Examples

Weekly on Monday and Wednesday until 12 Feb creates two occurrences per week until then.

## Exceptions

None.
