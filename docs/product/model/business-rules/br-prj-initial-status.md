---
id: BR-PRJ-INITIAL-STATUS
type: business-rule
title: "Initial status of a new project"
status: draft
applies-to:
  - "UC-CREATE-PROJECT"
  - "UC-PRJ-CREATE-FROM-TEMPLATE"
  - "UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/360013668139.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A new blank project starts in Draft; a project created from a request starts in Inquiry; a project created from a template starts in the template's status.

## Rationale

The starting stage depends on how the work came in.

## Examples

See SB-PRJ-BLANK-PROJECT-DRAFT.

## Exceptions

None.
