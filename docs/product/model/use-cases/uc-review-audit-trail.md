---
id: UC-REVIEW-AUDIT-TRAIL
type: use-case
title: "Review the audit trail of a record"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-COMPLIANCE"
governed-by:
  - "BR-AUDITED-ACTIONS"
uses-terms:
  - "TERM-AUDIT-EVENT"
---

## Goal

The administrator sees who did what to a draft, invoice or issuer profile, and when.

## Trigger

A question about the history of a record.

## Preconditions

None.

## Main Flow

1. The administrator opens a record.
2. The product lists its audit events in chronological order.

## Alternative Flows

None.

## Failure Conditions

- A user without permission to view audit events is refused and sees no events.
- A record without events shows an empty list.
- No audit event can be changed or deleted through the review (BR-AUDITED-ACTIONS).
- If the events cannot be loaded, the view fails visibly and can be retried; no partial history is presented as complete.

## Postconditions

No record is changed.
