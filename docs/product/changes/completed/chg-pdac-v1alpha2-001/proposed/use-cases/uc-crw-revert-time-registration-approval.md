---
id: UC-CRW-REVERT-TIME-REGISTRATION-APPROVAL
type: use-case
title: "Revert an approved registration to pending"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-APPROVED-READ-ONLY"
  - "BR-CRW-REGISTRATION-HISTORY"
uses-terms:
  - "TERM-TIME-REGISTRATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

A wrongly approved registration can be corrected.

## Trigger

A mistake is found after approval.

## Preconditions

The registration is approved.

## Main Flow

1. The planner reverts it to pending.
2. The planner corrects it.
3. The planner approves it again.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks the right to edit others' hours: the revert is refused and the registration stays approved (BR-CRW-APPROVED-READ-ONLY).
- The registration is not approved, for example already pending: there is nothing to revert and the request is refused.
- Another user changed or reverted the same registration in the meantime: the action is refused and the planner must reload and retry.
- Every revert and later change is recorded with who and when (BR-CRW-REGISTRATION-HISTORY).

## Postconditions

The registration is correct and approved.
