---
id: "SB-TRB-PROJECT-OUTSIDE-PERIOD"
type: structured-behaviour
title: "Project outside the selected period is not listed"
status: draft
illustrates:
  - "BR-TRB-OVERVIEW-PERIOD-OVERLAP"
  - "UC-PRJ-BROWSE-PROJECTS"
given:
  - "A festival project's planning period is 10 to 14 July"
  - "This week is 1 to 7 July"
when: "The coordinator shows the overview for this week"
then:
  - "The festival project is not listed"
  - "Selecting a period that includes 10 to 14 July lists it"
uses-terms:
  - "TERM-PROJECT-OVERVIEW"
provenance:
  source: ".private/research/help-center/articles/360014514680.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Typical cause of a 'missing' project.

## Boundaries

None.
