---
id: SB-PRJ-SIGNED-QUOTE-CONFIRMS
type: structured-behaviour
title: "A digital signature confirms the project"
status: active
illustrates:
  - "BR-PRJ-SIGNED-QUOTE-CONFIRMED"
given:
  - "A project is Pending with a sent quotation"
when: "The customer signs the quotation digitally"
then:
  - "The project status becomes Confirmed"
  - "The project's equipment becomes bookable in the warehouse"
uses-terms:
  - "TERM-PROJECT-STATUS-CONFIRMED"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Automatic transition on customer agreement.

## Boundaries

None.
