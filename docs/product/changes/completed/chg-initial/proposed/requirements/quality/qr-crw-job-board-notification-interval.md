---
id: QR-CRW-JOB-BOARD-NOTIFICATION-INTERVAL
type: quality-requirement
title: "Job board notifications at most every six hours"
status: draft
quality-attribute: "timeliness"
applies-to:
  - "UC-CRW-POST-JOB-ON-BOARD"
verification:
  - scenario: "A posting published at 09:00 is included in a notification sent no later than 15:00 to opted-in crew"
uses-terms:
  - "TERM-JOB-POSTING"
provenance:
  source: ".private/research/help-center/articles/13996376771730.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Crew MUST be notified of newly posted jobs in batches no more than six hours apart.

## Measurement

Interval between consecutive job board notifications per crew member; threshold six hours.
