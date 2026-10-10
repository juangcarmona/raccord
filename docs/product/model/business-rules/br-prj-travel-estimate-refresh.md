---
id: BR-PRJ-TRAVEL-ESTIMATE-REFRESH
type: business-rule
title: "Travel estimates are calculated on refresh"
status: draft
applies-to:
  - "UC-PRJ-ADD-CONTACT"
uses-terms:
  - "TERM-TRAVEL-ESTIMATE"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Distance and travel time of a contact are only calculated when explicitly refreshed, and only refreshed values are used when the contact is added to a project.

## Rationale

Avoids silent recalculation.

## Examples

A new venue without refresh shows no distance on projects.

## Exceptions

None.
