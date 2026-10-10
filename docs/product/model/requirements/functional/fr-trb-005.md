---
id: "FR-TRB-005"
type: functional-requirement
title: "Live updates and presence"
status: draft
derived-from:
  - "UC-TRB-SEE-LIVE-CHANGES"
  - "BR-TRB-LIVE-CHANGES-PUSHED"
  - "BR-TRB-PRESENCE-SHARED"
  - "BR-TRB-CONNECTION-LOST-WARNING"
verification:
  - scenario-ref: "SB-TRB-COLLEAGUE-CHANGE-APPEARS"
  - scenario-ref: "SB-TRB-PRESENCE-VISIBLE"
  - scenario-ref: "SB-TRB-CONNECTION-LOST"
uses-terms:
  - "TERM-LIVE-UPDATE-CONNECTION"
  - "TERM-RECORD-PRESENCE"
provenance:
  source: ".private/research/help-center/articles/360013988420.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST push saved changes to other users' open screens without reload, MUST show who is working in an open project, and MUST warn the user and recommend refreshing when the live connection is lost.

## Rationale

Simultaneous planning requires current data and awareness of colleagues.
