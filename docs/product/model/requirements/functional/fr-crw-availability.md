---
id: FR-CRW-AVAILABILITY
type: functional-requirement
title: "Derive and show crew availability"
status: draft
derived-from:
  - "UC-CRW-VIEW-CREW-AVAILABILITY"
  - "UC-CRW-ENTER-CREW-AVAILABILITY"
  - "UC-CRW-DECLARE-OWN-AVAILABILITY"
  - "BR-CRW-AVAILABILITY-DERIVATION"
  - "BR-CRW-PARTIAL-AVAILABILITY"
  - "BR-CRW-AVAILABILITY-RIGHTS"
verification:
  - scenario-ref: "SB-CRW-PARTIAL-UNAVAILABLE"
  - scenario-ref: "SB-CRW-PARTIAL-AVAILABLE-UNKNOWN"
  - scenario-ref: "SB-CRW-APPOINTMENT-MAKES-UNAVAILABLE"
  - scenario-ref: "SB-CRW-UNANSWERED-INVITATION-SHOWS-INVITED"
uses-terms:
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/360014685859.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST derive each resource's availability for a period from planning, shifts, appointments, declarations, invitations, and reservations, and MUST let crew and authorised planners declare availability with recurrence.

## Rationale

Informed staffing.
