---
id: SB-PRJ-CANCELLED-WITH-CREW-IN-PLANNER
type: structured-behaviour
title: "Cancelled projects stay in crew planning while crew is planned"
status: draft
illustrates:
  - "BR-PRJ-STATUS-AREA-VISIBILITY"
given:
  - "A project is cancelled"
  - "Two crew members are still planned on its functions"
when: "The crew planner opens crew planning"
then:
  - "The cancelled project is still shown"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Prevents crew being forgotten on cancelled jobs.

## Boundaries

Once crew is removed the project is hidden there.
