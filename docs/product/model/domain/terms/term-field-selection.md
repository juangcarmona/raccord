---
id: TERM-FIELD-SELECTION
type: domain-term
title: "Field selection"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-GENERATED-FIELD"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A request option naming which fields of a record the response must contain, including custom fields by their custom-field number. Generated fields are returned in collections only when selected.

## Distinguish From

Not a filter: it narrows columns, not records.

## Usage

Used to reduce response size and to obtain generated fields.
