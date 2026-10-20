---
id: BR-PRJ-CANCEL-MANUAL-ONLY
type: business-rule
title: "Cancellation is never automatic"
status: active
applies-to:
  - "UC-PRJ-CANCEL-PROJECT"
uses-terms:
  - "TERM-PROJECT-STATUS-CANCELLED"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The Cancelled status is only ever set by a user; no event moves a project to Cancelled automatically.

## Rationale

Cancelling has wide consequences on crew and equipment.

## Examples

An expired quote does not cancel the project.

## Exceptions

None.
