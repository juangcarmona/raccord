---
id: BR-PRJ-FIXED-STATUS-SET
type: business-rule
title: "The project status set is fixed"
status: active
applies-to:
  - "UC-PRJ-CHANGE-PROJECT-STATUS"
  - "BC-PROJECTS"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Users cannot add their own project statuses, and there is no status meaning invoiced; whether a project is fully invoiced is derived from whether all its invoice moments are completed.

## Rationale

Invoicing timing differs per company and project and can precede the equipment's return.

## Examples

A returned project with an open final invoice moment is not fully invoiced; a confirmed project fully invoiced in advance is.

## Exceptions

Equipment handling in the warehouse supports its own custom statuses.
