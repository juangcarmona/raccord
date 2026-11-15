---
id: UC-CRW-SIGN-UP-FOR-POSTED-JOB
type: use-case
title: "Sign up for a posted job"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-JOB-BOARD-IGNORES-CONFLICTS"
uses-terms:
  - "TERM-JOB-POSTING"
provenance:
  source: ".private/research/help-center/articles/17152009951634.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

The crew member takes a job.

## Trigger

An interesting posting.

## Preconditions

Postings exist.

## Main Flow

1. The crew member browses postings, sorted by due date, function time or recency.
2. They open one and sign up.
3. The product plans them on the function.

## Alternative Flows

They dismiss a posting from their view.

## Failure Conditions

- The crew member lacks the right to sign up for postings: the request is refused and they are not planned.
- The posting was removed or the function was already fully staffed by someone else in the meantime: the sign-up is refused and the crew member is told the posting is no longer open.
- Signing up does not check the crew member's availability or conflicting planning, so a double booking is not refused here; the planner remains responsible for resolving it (BR-CRW-JOB-BOARD-IGNORES-CONFLICTS).
- No postings exist: the list is shown empty.

## Postconditions

The crew member is planned on the function.
