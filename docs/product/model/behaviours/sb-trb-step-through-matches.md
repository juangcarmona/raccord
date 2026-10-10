---
id: "SB-TRB-STEP-THROUGH-MATCHES"
type: structured-behaviour
title: "Stepping through highlighted matches"
status: draft
illustrates:
  - "BR-TRB-IN-PROJECT-SEARCH-HIGHLIGHT"
given:
  - "A project has four planned lines containing 'cable'"
when: "The coordinator searches the project for 'cable' and steps to the next match"
then:
  - "All four cable lines are highlighted"
  - "The selected match moves from the first to the second line and is marked more strongly"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015031799.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Navigation among matches.

## Boundaries

None.
