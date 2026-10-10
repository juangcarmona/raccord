---
id: FR-WHS-044
type: functional-requirement
title: "Availability timeline: project holds"
status: draft
derived-from:
  - "UC-WHS-VIEW-AVAILABILITY-TIMELINE"
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
verification:
  - scenario: "A pending project shows a soft hold over its planning period, and a confirmed project shows a reserved hold"
uses-terms:
  - "TERM-RESERVATION-STATE"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360012699859.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST show on the availability timeline every project that holds the selected articles, distinguishing its planning period from its usage period and showing whether the quantity is unreserved, partially or fully reserved.

## Rationale

Split from FR-WHS-013 so each timeline obligation can be verified on its own.
