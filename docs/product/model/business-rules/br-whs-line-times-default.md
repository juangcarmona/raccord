---
id: BR-WHS-LINE-TIMES-DEFAULT
type: business-rule
title: "Lines inherit default times until changed"
status: draft
applies-to:
  - "UC-WHS-SET-LINE-TIMES"
  - "UC-WHS-PLAN-EQUIPMENT"
  - "UC-WHS-ORGANIZE-GROUPS"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
  - "TERM-EQUIPMENT-GROUP"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md, .private/research/help-center/articles/360017647460.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A newly planned line takes the default planning and usage periods of its equipment group, or of the project when the group has none; changing a group's times applies them to all lines of the group; a line keeps an individually set period until changed again.

## Rationale

Only deviations should need manual work.

## Examples

See SB-WHS-LINE-INHERITS-SUBPROJECT-TIMES.

## Exceptions

None.
