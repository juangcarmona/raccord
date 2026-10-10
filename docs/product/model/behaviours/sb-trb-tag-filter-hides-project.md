---
id: "SB-TRB-TAG-FILTER-HIDES-PROJECT"
type: structured-behaviour
title: "Active tag filter hides an untagged project"
status: draft
illustrates:
  - "BR-PRJ-TAG-FILTER-MODES"
  - "UC-PRJ-BROWSE-PROJECTS"
given:
  - "A tag filter on 'festival' is active"
  - "The wanted project has no tags"
when: "The coordinator searches the overview for that project"
then:
  - "The project is not shown"
  - "Clearing the tag filter shows it"
uses-terms:
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360014514680.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Active filters are a frequent cause of missing results.

## Boundaries

None.
