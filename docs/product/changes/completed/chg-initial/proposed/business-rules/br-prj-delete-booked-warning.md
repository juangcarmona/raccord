---
id: BR-PRJ-DELETE-BOOKED-WARNING
type: business-rule
title: "Deleting a project with booked-out equipment warns"
status: draft
applies-to:
  - "UC-PRJ-DELETE-PROJECT"
uses-terms:
  - "TERM-PROJECT"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Deleting a project that has equipment booked out of the warehouse produces a warning that booked equipment does not fit the project's status.

## Rationale

Equipment physically out must stay traceable.

## Examples

A project with five items on location is deleted: the warning appears.

## Exceptions

None.
