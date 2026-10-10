---
id: BR-EQP-SCAN-CODE-GRANULARITY
type: business-rule
title: "Items share one scan code"
status: draft
applies-to:
  - "UC-WHS-ASSIGN-SCAN-CODES"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/360017649020.md, .private/research/help-center/articles/360013086279.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An item has a single scan code shared by all its pieces; an individual piece cannot have its own code.

## Rationale

Items are counted by quantity, so pieces need not be told apart.

## Examples

- Four microphone stands carry the same label.

## Exceptions

None.
