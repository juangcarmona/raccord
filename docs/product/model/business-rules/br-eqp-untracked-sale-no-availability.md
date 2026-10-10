---
id: BR-EQP-UNTRACKED-SALE-NO-AVAILABILITY
type: business-rule
title: "Untracked sale items have no availability"
status: draft
applies-to:
  - "UC-EQP-CREATE-SALE-ITEM"
uses-terms:
  - "TERM-STOCK-MANAGEMENT-MODE"
provenance:
  source: ".private/research/help-center/articles/13021031495698.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A sale item excluded from stock tracking has no stock or availability, never shows a shortage and cannot have a critical stock level.

## Rationale

Cheap consumables always in supply need no tracking.

## Examples

- Gaffer tape planned in any quantity shows no shortage.

## Exceptions

None.
