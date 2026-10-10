---
id: FR-INT-014
type: functional-requirement
title: "Publish personal schedules as calendar feeds"
status: draft
derived-from:
  - "UC-INT-SUBSCRIBE-TO-CALENDAR-FEED"
  - "BR-INT-SYNC-RESPECTS-ROLE"
  - "BR-INT-SYNC-WINDOW-SIX-MONTHS"
  - "BR-INT-FEED-ONE-WAY"
  - "BR-INT-CANCELLED-MARKED-IN-SYNC"
verification:
  - scenario-ref: SB-INT-SYNC-HIDES-OTHERS-FUNCTIONS
  - scenario-ref: SB-INT-SYNC-WINDOW-EDGE
  - scenario-ref: SB-INT-FEED-NOT-WRITABLE
  - scenario-ref: SB-INT-CANCELLED-STRUCK
uses-terms:
  - "TERM-CALENDAR-STREAM"
provenance:
  source: ".private/research/help-center/articles/360014047779.md, .private/research/help-center/articles/21998749265938.md, .private/research/help-center/articles/14042545177618.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST publish each user's schedule as selectable streams (functions, own functions, own and public appointments, projects, reservations, own and public tasks) through private read-only subscription feeds, limited to the user's permissions and a six-month window.

## Rationale

People plan their lives in their own calendars.
