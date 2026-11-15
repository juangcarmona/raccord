---
id: UC-CRW-RECONCILE-PLANNED-AND-REGISTERED-TIME
type: use-case
title: "Compare planned and registered time and cost"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-RECONCILIATION-STATUS-DERIVATION"
  - "BR-CRW-ACTUAL-COST-FROM-REGISTRATIONS"
  - "BR-CRW-OVERTIME-FROM-REGISTERED-HOURS"
  - "BR-CRW-RATE-FACTOR-CALCULATION"
  - "BR-CRW-BREAK-AND-TRAVEL-EXCLUDED"
  - "BR-CRW-PLANNED-COST-VISIBILITY"
uses-terms:
  - "TERM-RECONCILIATION-STATUS"
  - "TERM-PLANNED-COST"
  - "TERM-ACTUAL-COST"
provenance:
  source: ".private/research/help-center/articles/23814779870610.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Differences between plan and reality are visible.

## Trigger

Payroll, budget or customer questions.

## Preconditions

Registrations and planned activities exist.

## Main Flow

1. The user views activities per project, registration or crew member.
2. The product shows planned, registered and difference time, planned, actual and difference cost, and reconciliation status.

## Alternative Flows

None.

## Failure Conditions

- The user lacks permission to see activities or hours: the view is refused and nothing is shown.
- The user lacks the role right to see planned cost: time is shown but planned cost is withheld (BR-CRW-PLANNED-COST-VISIBILITY).
- A past activity has no registration: it is shown as no registration rather than with a time difference (BR-CRW-RECONCILIATION-STATUS-DERIVATION).
- No activities match the chosen scope: the view is shown empty.

## Postconditions

No change.
