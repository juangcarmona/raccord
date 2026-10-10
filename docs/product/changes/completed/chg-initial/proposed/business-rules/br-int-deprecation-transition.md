---
id: BR-INT-DEPRECATION-TRANSITION
type: business-rule
title: "Moved fields are served in both places during a transition"
status: draft
applies-to:
  - "UC-INT-ADAPT-TO-API-CHANGE"
uses-terms:
  - "TERM-API-DEPRECATION"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a field moves between resources, it is returned with identical values in both places until the announced end of the transition, after which only the new place remains.

## Rationale

Integrations need time to adapt.

## Examples

Planning and usage period fields move from equipment groups to individual planned equipment and remain on both until the announced quarter.

## Exceptions

Changes flagged as breaking.
