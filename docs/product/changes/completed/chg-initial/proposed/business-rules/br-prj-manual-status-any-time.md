---
id: BR-PRJ-MANUAL-STATUS-ANY-TIME
type: business-rule
title: "Project status can be changed manually within its lifecycle"
status: draft
applies-to:
  - "UC-PRJ-CHANGE-PROJECT-STATUS"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/360013668139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A user may change a project status manually when the transition is allowed by the project's lifecycle. Complete prevents operational edits and new planning; leaving Complete is an explicit reopening action and does not silently reopen operational work. Cancelled continues to trigger the cancellation workflow.

## Rationale

Automatic transitions do not cover all real situations.

## Examples

A project confirmed by phone is set to Confirmed directly from Draft.

## Exceptions

Cancelled triggers the cancellation workflow. Complete requires explicit reopening before operational changes or new planning.
