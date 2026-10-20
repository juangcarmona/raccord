---
id: FR-PRJ-015
type: functional-requirement
title: "Create and process project requests"
status: active
derived-from:
  - "UC-PRJ-RECEIVE-PROJECT-REQUEST"
  - "UC-PRJ-CONVERT-PROJECT-REQUEST"
  - "UC-PRJ-DECLINE-PROJECT-REQUEST"
  - "BR-PRJ-REQUEST-NOTIFICATION"
verification:
  - scenario: "A request cannot be turned into a project until a client and location are chosen"
  - scenario: "Declining a request e-mails the staff member who created it"
  - scenario-ref: "SB-PRJ-DECLINE-NOTIFIES-REQUESTER"
  - scenario-ref: "SB-PRJ-SAVED-MATCH-SUGGESTED"
uses-terms:
  - "TERM-PROJECT-REQUEST"
  - "TERM-PROJECT-REQUEST-STATUS"
provenance:
  source: ".private/research/help-center/articles/4403455031186.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let staff create project requests, notify account managers, let users choose client, location and equipment to create a project from a request, or decline it with notice to the requester.

## Rationale

Staff can ask for projects to be set up.
