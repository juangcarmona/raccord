---
id: SB-INT-LEAVE-BEHIND
type: structured-behaviour
title: "Left-behind item removed, moment continues"
status: draft
illustrates:
  - "BR-INT-LEAVE-BEHIND-REMOVES-LINE"
  - "UC-INT-LEAVE-EQUIPMENT-BEHIND"
given:
  - "a packing moment holds four lights"
  - "three are prepped"
when: "the technician marks one damaged light as left behind"
then:
  - "the light is removed from the project"
  - "the packing moment status is determined by the remaining items"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/37629880908050.md"
  confidence: high
  recovered-from: interview
---

## Intent

Leaving equipment behind.

## Boundaries

None.
