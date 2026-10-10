---
id: FR-CRW-JOB-BOARD
type: functional-requirement
title: "Job board"
status: draft
derived-from:
  - "UC-CRW-POST-JOB-ON-BOARD"
  - "UC-CRW-SIGN-UP-FOR-POSTED-JOB"
  - "BR-CRW-JOB-BOARD-IGNORES-CONFLICTS"
  - "BR-CRW-JOB-BOARD-NOTIFICATION-CADENCE"
verification:
  - scenario-ref: "SB-CRW-JOB-SIGNUP-DESPITE-CONFLICT"
  - scenario: "Five functions posted within one morning produce a single notification to each opted-in crew member"
uses-terms:
  - "TERM-JOB-POSTING"
provenance:
  source: ".private/research/help-center/articles/13996376771730.md, .private/research/help-center/articles/17152009951634.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST let planners post and remove functions on a job board that crew browse, sort, dismiss and sign up for on the web and in the mobile web app, notifying crew in batches.

## Rationale

Self-service staffing.
