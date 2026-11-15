---
id: UC-CRW-APPROVE-TIME-REGISTRATIONS
type: use-case
title: "Review and approve time registrations"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-APPROVAL-ROUTING"
  - "BR-CRW-APPROVED-READ-ONLY"
  - "BR-CRW-PENDING-NOT-IN-TOTAL"
  - "BR-CRW-APPROVAL-CHECK-THRESHOLDS"
  - "BR-CRW-MINIMUM-PAID-HOURS-CHECK"
  - "BR-CRW-REGISTRATION-HISTORY"
uses-terms:
  - "TERM-TIME-REGISTRATION-STATUS"
  - "TERM-APPROVAL-CHECK"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Only verified hours reach payroll.

## Trigger

Registrations are pending.

## Preconditions

Approval is enabled; the user may edit others' hours.

## Main Flow

1. The planner lists pending registrations with approval check signals.
2. The planner adjusts data where needed.
3. The planner approves individually, in bulk or stepping through registrations.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks the right to edit or approve others' hours: the request is refused and the registrations stay pending (BR-CRW-TIME-REGISTRATION-RIGHTS).
- A registration is already approved and the planner tries to change it: it is refused until the registration is reverted to pending, after which it must be approved again (BR-CRW-APPROVED-READ-ONLY).
- Another user changed or decided the same registration in the meantime: the action is refused and the planner must reload and retry.
- Approval is not enabled for the hour type: there is nothing pending to approve, and the list is empty (BR-CRW-APPROVAL-ROUTING).

## Postconditions

Approved registrations are read-only and counted; owners may be notified of changes.
