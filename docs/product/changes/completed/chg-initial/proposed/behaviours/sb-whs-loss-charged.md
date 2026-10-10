---
id: SB-WHS-LOSS-CHARGED
type: structured-behaviour
title: "Charging a loss adds a project cost"
status: draft
illustrates:
  - "BR-WHS-CHARGE-TO-PROJECT"
  - "UC-WHS-MARK-LOST"
when: "A technician marks 1 microphone lost and charges 'Lost microphone' 250 to the customer"
then:
  - "The project gets an additional cost 'Lost microphone' of 250"
  - "The lost record shows 250 as sale value"
uses-terms:
  - "TERM-DAMAGE-CHARGE"
provenance:
  source: ".private/research/help-center/articles/17531415206418.md, .private/research/help-center/articles/360014392200.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Losses reach billing.

## Boundaries

None.
