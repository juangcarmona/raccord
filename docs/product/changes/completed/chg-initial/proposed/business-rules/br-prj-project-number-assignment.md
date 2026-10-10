---
id: BR-PRJ-PROJECT-NUMBER-ASSIGNMENT
type: business-rule
title: "Project numbers are unique and auto-assigned when empty"
status: draft
applies-to:
  - "UC-PRJ-EDIT-PROJECT-DETAILS"
  - "UC-CREATE-PROJECT"
  - "UC-PRJ-CREATE-FROM-TEMPLATE"
  - "UC-PRJ-DUPLICATE-PROJECT"
uses-terms:
  - "TERM-PROJECT-NUMBER"
  - "TERM-NUMBER-SERIES"
provenance:
  source: ".private/research/help-center/articles/360013668139.md, .private/research/help-center/articles/29017857097618.md, .private/research/help-center/articles/360014106599.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A project whose number is left empty receives the next number of the project number series; a custom number may be entered; no two projects share a number.

## Rationale

Numbers identify projects in documents, e-mails and imports.

## Examples

Series current value 2041: the next blank-number project gets 2042.

## Exceptions

None.
