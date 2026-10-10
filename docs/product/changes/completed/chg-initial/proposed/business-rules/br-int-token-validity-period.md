---
id: BR-INT-TOKEN-VALIDITY-PERIOD
type: business-rule
title: "Tokens are valid for ten years"
status: draft
applies-to:
  - "UC-INT-ENABLE-API-ACCESS"
uses-terms:
  - "TERM-API-TOKEN-VALIDITY"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/30759527977490.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A newly created API token expires ten years after creation; tokens created before this rule keep their original five-year validity.

## Rationale

Long-lived integrations should not break unexpectedly, while tokens still end eventually.

## Examples

A token created on 2026-03-01 stops working on 2036-03-01.

## Exceptions

Tokens created under the earlier rule.
