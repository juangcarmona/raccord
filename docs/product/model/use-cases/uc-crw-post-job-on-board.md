---
id: UC-CRW-POST-JOB-ON-BOARD
type: use-case
title: "Post functions on the job board"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
  - "ACT-RACCORD-AUTOMATION"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-JOB-BOARD-NOTIFICATION-CADENCE"
uses-terms:
  - "TERM-JOB-POSTING"
provenance:
  source: ".private/research/help-center/articles/13996376771730.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Crew can sign up for open functions themselves.

## Trigger

The exact person does not matter.

## Preconditions

The project has functions.

## Main Flow

1. The planner selects functions and posts them with details.
2. The product publishes them; crew are notified in the next batch.

## Alternative Flows

The planner removes selected postings in bulk.

## Failure Conditions

- The planner lacks permission to post jobs: the postings are refused and nothing is published.
- No function is selected, or a selected function lacks the details needed to sign up, such as times: it is refused with the reason and not posted.
- Crew who opted out of job notifications are not notified of the postings, and others are notified at most every six hours rather than at once (BR-CRW-JOB-BOARD-NOTIFICATION-CADENCE).
- The notification service is unreachable: the postings stay published but the notice fails visibly and can be retried.

## Postconditions

Postings are visible to crew on the web and in the mobile web app.
