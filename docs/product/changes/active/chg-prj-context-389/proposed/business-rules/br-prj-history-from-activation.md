---
id: BR-PRJ-HISTORY-FROM-ACTIVATION
type: business-rule
title: "History starts at activation"
status: active
applies-to:
  - "UC-PRJ-REVIEW-HISTORY"
uses-terms:
  - "TERM-HISTORY-LOG"
provenance:
  source: ".private/research/help-center/articles/4405951357202.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Changes are recorded only from the moment change history is activated; earlier changes are never available.

## Rationale

Recording is not retroactive.

## Examples

A project edited before activation shows no earlier saves.

## Exceptions

None.
