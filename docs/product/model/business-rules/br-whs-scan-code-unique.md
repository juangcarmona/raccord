---
id: BR-WHS-SCAN-CODE-UNIQUE
type: business-rule
title: "Scan codes are unique and at most 25 characters"
status: draft
applies-to:
  - "UC-WHS-ASSIGN-SCAN-CODES"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/360013101260.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Every scan code must be unique across articles and combinations; codes longer than 25 characters are truncated, which can make them collide, so they must be kept within 25 characters.

## Rationale

A scan must resolve to one thing.

## Examples

See SB-WHS-LONG-CODE-TRUNCATED.

## Exceptions

None.
