---
id: BR-PRJ-PROJECT-TYPE-DEFAULTS
type: business-rule
title: "Project types supply defaults"
status: active
applies-to:
  - "UC-PRJ-MANAGE-PROJECT-TYPES"
  - "UC-PRJ-EDIT-PROJECT-DETAILS"
uses-terms:
  - "TERM-PROJECT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013631800.md, .private/research/help-center/articles/360013668139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A project takes from its type the default colour, document templates, letterhead, number series for financial documents, invoice moment and additional conditions.

## Rationale

Correct settings without manual choices.

## Examples

A 'Sale' type sets the sale invoice template and series.

## Exceptions

Client commercial defaults override the invoice moment.
