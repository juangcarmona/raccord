---
id: SB-INT-CUSTOM-FIELD-FILTER
type: structured-behaviour
title: "Filter and expand on a custom field"
status: draft
illustrates:
  - "BR-INT-CUSTOM-FIELDS-QUERYABLE"
given:
  - "projects have a custom field linking to a crew member"
when: "an application lists projects filtered on that custom field and expands it"
then:
  - "only projects whose field equals the given crew member are returned"
  - "each returned project shows the crew member record inline instead of its reference"
uses-terms:
  - "TERM-CUSTOM-FIELD"
  - "TERM-RELATION-EXPANSION"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Custom fields are first-class in queries.

## Boundaries

None.
