---
id: SB-PRJ-BLANK-PROJECT-DRAFT
type: structured-behaviour
title: "A blank project starts in Draft"
status: draft
illustrates:
  - "BR-PRJ-INITIAL-STATUS"
when: "A coordinator saves a new blank project with its schedule"
then:
  - "The project status is Draft"
  - "The project appears in the unconfirmed group"
uses-terms:
  - "TERM-PROJECT-STATUS-DRAFT"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Default entry state of manual creation.

## Boundaries

None.
