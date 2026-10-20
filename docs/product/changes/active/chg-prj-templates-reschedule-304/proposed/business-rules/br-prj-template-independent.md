---
id: BR-PRJ-TEMPLATE-INDEPENDENT
type: business-rule
title: "Templates survive deletion of their source project"
status: active
applies-to:
  - "UC-PRJ-CREATE-PROJECT-TEMPLATE"
  - "UC-PRJ-DELETE-PROJECT"
uses-terms:
  - "TERM-PROJECT-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/360013367659.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Deleting the project a template was copied from does not affect the template.

## Rationale

Templates hold their own copy.

## Examples

A helper project created only to build a template is deleted; the template keeps working.

## Exceptions

None.
