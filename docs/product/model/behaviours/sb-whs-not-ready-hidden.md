---
id: SB-WHS-NOT-READY-HIDDEN
type: structured-behaviour
title: "Unready projects are hidden from the crew"
status: draft
illustrates:
  - "BR-WHS-READY-TO-PACK-VISIBILITY"
  - "UC-WHS-MARK-READY-TO-PACK"
given:
  - "A confirmed project is not ready to pack"
  - "Technician T's role lacks the see-all permission"
when: "T opens the board on its due date"
then:
  - "T does not see its packing lists"
uses-terms:
  - "TERM-READY-TO-PACK"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Handover gate.

## Boundaries

Users with the see-all permission do see it.
