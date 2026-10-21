---
id: BR-PRJ-LINKED-PERIOD-PROPAGATION
type: business-rule
title: "Linked planning follows its period"
status: active
applies-to:
  - "UC-PRJ-MANAGE-TIME-SCHEDULE"
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
uses-terms:
  - "TERM-TIME-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360013599680.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment groups and crew or transport functions linked to a time period take that period's times, so any change to the period is applied to everything linked to it.

## Rationale

One change of date must not require editing every group and function.

## Examples

Moving the build period one day later moves the build function group with it.

## Exceptions

Items with their own unlinked times are not moved.
