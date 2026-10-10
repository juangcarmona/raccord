---
id: BR-CRW-FUNCTION-TIMING-PROPAGATION
type: business-rule
title: "Functions linked to the time schedule follow its changes"
status: draft
applies-to:
  - "UC-CRW-CREATE-FUNCTION-GROUP"
  - "UC-CRW-ADD-FUNCTION-TO-PROJECT"
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
  - "UC-PRJ-MANAGE-TIME-SCHEDULE"
uses-terms:
  - "TERM-FUNCTION-TIMING-MODE"
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/14319493249170.md, .private/research/help-center/articles/14232209807506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A function or function group linked to time-schedule periods takes its times from those periods, so changing the schedule moves the linked functions without editing them.

## Rationale

Avoids retyping times for every function.

## Examples

Moving the show period one hour later moves all show functions one hour later.

## Exceptions

Functions with own times or a duration are not moved.
