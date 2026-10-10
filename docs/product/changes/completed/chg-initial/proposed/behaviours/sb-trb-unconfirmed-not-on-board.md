---
id: "SB-TRB-UNCONFIRMED-NOT-ON-BOARD"
type: structured-behaviour
title: "Unconfirmed project is not on the to-pack list"
status: draft
illustrates:
  - "BR-WHS-BOARD-DUE-DATE"
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
given:
  - "A project's planning period starts today"
  - "The project is still in an option status"
when: "The warehouse technician opens today's board"
then:
  - "The project is not in the to-pack list"
  - "After the project is confirmed it appears"
uses-terms:
  - "TERM-PROJECT-STATUS-CONFIRMED"
  - "TERM-WAREHOUSE-DUE-DATE"
provenance:
  source: ".private/research/help-center/articles/360015018360.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Only confirmed work is packed.

## Boundaries

None.
