---
id: FR-PRJ-003
type: functional-requirement
title: "Maintain time schedules with planning and usage periods"
status: active
derived-from:
  - "UC-PRJ-MANAGE-TIME-SCHEDULE"
  - "BR-PRJ-DEFAULT-PERIODS"
  - "BR-PRJ-USAGE-WITHIN-PLANNING"
  - "BR-PRJ-LINKED-PERIOD-PROPAGATION"
  - "BR-PRJ-PERIOD-DOCUMENT-DISPLAY"
  - "BR-PRJ-PERIOD-EDIT-AVAILABILITY"
verification:
  - scenario: "A new schedule contains one planning and one usage period"
  - scenario: "Moving a period moves the equipment group linked to it"
  - scenario-ref: "SB-PRJ-PERIOD-EDIT-CONFLICT-REFUSED"
uses-terms:
  - "TERM-TIME-SCHEDULE"
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360013599680.md, .private/research/help-center/articles/360017700339.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST keep a schedule of named periods per project, with designated planning and usage periods, per-period document display settings, manual or chronological ordering, duplication, and propagation of period changes to linked groups and functions.

## Rationale

Periods drive availability, pricing and documents.
