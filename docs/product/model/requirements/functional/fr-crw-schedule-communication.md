---
id: FR-CRW-SCHEDULE-COMMUNICATION
type: functional-requirement
title: "Communicate schedules"
status: draft
derived-from:
  - "UC-CRW-SHARE-SCHEDULE"
  - "UC-CRW-GENERATE-PERSONAL-SCHEDULE-DOCUMENT"
  - "UC-CRW-GENERATE-CALL-SHEET"
  - "BR-CRW-SCHEDULE-PERSONAL-CONTENT"
  - "BR-CRW-CHANGED-SINCE-LAST-SHARE"
  - "BR-CRW-NOTIFICATION-PREFERENCES-SELF-OWNED"
verification:
  - scenario-ref: "SB-CRW-SHARED-SCHEDULE-PERSONAL"
  - scenario-ref: "SB-CRW-CHANGE-NOT-NOTIFIED"
  - scenario: "A call sheet generated for a project lists the project schedule and the planning of every crew member and vehicle on it"
uses-terms:
  - "TERM-SCHEDULE-COMMUNICATION"
  - "TERM-CALL-SHEET"
provenance:
  source: ".private/research/help-center/articles/360014672919.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let planners share personal schedules through each person's chosen channels, mark crew changed since last sharing, and generate personal schedule documents and project call sheets.

## Rationale

Crew must know where and when to work.
