---
id: BR-INT-CUSTOM-FIELDS-QUERYABLE
type: business-rule
title: "Custom fields behave like standard fields in queries"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
uses-terms:
  - "TERM-CUSTOM-FIELD"
  - "TERM-API-FILTER"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Custom fields can be filtered with the same operators as standard fields, selected by their number, and, when they point to a record, expanded inline.

## Rationale

Companies rely on custom fields to model their own data.

## Examples

Filtering projects on a yes/no custom field returns only those set to yes.

## Exceptions

None.
