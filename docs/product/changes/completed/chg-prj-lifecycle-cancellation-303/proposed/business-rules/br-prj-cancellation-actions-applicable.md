---
id: BR-PRJ-CANCELLATION-ACTIONS-APPLICABLE
type: business-rule
title: "Only applicable cancellation actions are offered"
status: active
applies-to:
  - "UC-PRJ-CANCEL-PROJECT"
uses-terms:
  - "TERM-CANCELLATION-ACTION"
provenance:
  source: ".private/research/help-center/articles/28083930405778.md"
  confidence: high
  recovered-from: documentation
---

## Rule

On cancellation the product offers each cancellation action only if it applies to the project (for example completing tasks only when open tasks exist), and performs exactly the actions the user confirms.

## Rationale

Avoids meaningless choices.

## Examples

A project without tasks is cancelled: completing tasks is not offered.

## Exceptions

The user may decline all actions.
