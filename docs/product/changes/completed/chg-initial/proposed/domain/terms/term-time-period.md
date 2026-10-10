---
id: TERM-TIME-PERIOD
type: domain-term
title: "Time period"
status: draft
defined-in: "BC-PROJECTS"
synonyms: []
uses-terms:
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/360013599680.md, .private/research/help-center/articles/360013677019.md, .private/research/help-center/articles/360017700339.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A named interval with a start date-time and an end date-time inside a time schedule. Attributes: name, start, end, position in the schedule, the items linked to it (equipment groups, crew or transport functions), and whether it is displayed on internal documents and on external documents. A period belongs to one project. A period may be designated as the default planning period, the default usage period, or both.

## Distinguish From

Task deadline: a due moment without a duration. Function: a crew role that is linked to a period but is not itself a period.

## Usage

Linked items reuse the period's times, so changing a period moves everything linked to it.
