---
id: BR-EQP-NESTING-LIMIT
type: business-rule
title: "Kits nest at most ten levels deep"
status: draft
applies-to:
  - "UC-EQP-NEST-KITS"
uses-terms:
  - "TERM-NESTED-KIT"
provenance:
  source: ".private/research/help-center/articles/19934846796306.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A kit may contain other kits as content, in the catalog or on a project, up to a maximum of 10 nesting levels.

## Rationale

Bounds structural complexity.

## Examples

- A box inside a case inside a truck pack is three levels and allowed.

## Exceptions

None.
