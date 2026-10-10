---
id: SB-WHS-READY-TO-PACK-LOCKED
type: structured-behaviour
title: "Ready flag locks after first booking"
status: draft
illustrates:
  - "BR-WHS-READY-TO-PACK-LOCK"
given:
  - "A project is ready to pack and one cable has been booked to prepped"
when: "The coordinator switches ready to pack off"
then:
  - "The change is refused"
uses-terms:
  - "TERM-READY-TO-PACK"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Prevents hiding work in progress.

## Boundaries

None.
