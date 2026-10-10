---
id: BR-INT-GENERATED-FIELDS-ON-REQUEST
type: business-rule
title: "Generated fields appear in collections only when selected"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
uses-terms:
  - "TERM-GENERATED-FIELD"
  - "TERM-FIELD-SELECTION"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Computed fields such as project cost figures are omitted from collection responses unless the request selects them.

## Rationale

Computing them for every record of a page is costly.

## Examples

Listing projects returns no planned cost until the field is selected.

## Exceptions

None.
