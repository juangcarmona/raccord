---
id: TERM-PLAN-STATUS
type: domain-term
title: "Plan status"
status: draft
defined-in: "BC-CREW"
synonyms:
  - "visibility"
uses-terms:
  - "TERM-CREW-ASSIGNMENT"
  - "TERM-DRAFT-PLANNING"
  - "TERM-FINAL-PLANNING"
provenance:
  source: ".private/research/help-center/articles/360014607700.md, .private/research/help-center/articles/360014610140.md, .private/research/help-center/articles/360014594620.md, .private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

Lifecycle of a crew assignment's visibility to the planned person.

States: draft (invisible to the crew member; the planner can rework freely) and final (visible in the crew member's personal schedule).

Transitions:

| From | To | Trigger |
| --- | --- | --- |
| (new assignment) | default plan status of the context | planner plans directly |
| (new assignment) | final | crew member accepts a final-planning invitation |
| (new assignment) | draft | crew member accepts a draft-planning invitation |
| draft | final | planner finalises one assignment or the whole function |
| final | draft | planner switches the assignment back |

## Distinguish From

Staffing status: function-level fulfilment. Schedule communication: sending the plan, which is separate from visibility.

## Usage

Deciding what crew see; counting hours planned visibly and invisibly.
