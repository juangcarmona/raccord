---
id: UC-WSP-RECEIVE-NOTIFICATION
type: use-case
title: "Notify a user of a relevant event"
status: draft
primary-actor: "ACT-RACCORD-AUTOMATION"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-NOTIFICATION-CHANNEL-CHOICE"
  - "BR-WSP-IMPORTANT-DAYS-NON-BLOCKING"
uses-terms:
  - "TERM-NOTIFICATION"
  - "TERM-NOTIFICATION-CATEGORY"
provenance:
  source: ".private/research/help-center/articles/360019724320.md, .private/research/help-center/articles/19386134798866.md, .private/research/help-center/articles/360018840939.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

The concerned user learns about the event promptly.

## Trigger

An event of a notification category occurs (invitation, project change, financial update, task, maintenance, email, changed hours, important day).

## Preconditions

- The user is concerned by the event and has channels enabled for its category.

## Main Flow

1. The product determines the users concerned.
2. The product sends the notification through each enabled channel.
3. Web notifications appear under the user's notification list.

## Alternative Flows

- None.

## Failure Conditions

- The phone or browser blocks notifications: the phone notification is not displayed.

## Postconditions

- The user has been informed.
