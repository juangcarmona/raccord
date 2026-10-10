---
id: BR-CRW-AVAILABILITY-ADVISORY
type: business-rule
title: "Unknown availability does not prevent planning"
status: draft
applies-to:
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
uses-terms:
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/360014685859.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Planning a crew member does not require known availability; availability information guides the planner.

## Rationale

Many crew never declare availability.

## Examples

A crew member with no entries can be planned directly.

## Exceptions

An explicitly unavailable crew member may still be planned, but the product shows a clear warning and requires confirmation.
