---
id: UC-INT-SUBSCRIBE-TO-CALENDAR-FEED
type: use-case
title: "Subscribe a calendar application to a schedule feed"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors:
  - "ACT-EXTERNAL-APPLICATION"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-INT-FEED-ONE-WAY"
  - "BR-INT-SYNC-RESPECTS-ROLE"
uses-terms:
  - "TERM-CALENDAR-SUBSCRIPTION-FEED"
  - "TERM-CALENDAR-STREAM"
provenance:
  source: ".private/research/help-center/articles/360014047779.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user's planning is visible in a calendar application through a subscription.

## Trigger

The user wants the schedule in a calendar application.

## Preconditions

None beyond being a user.

## Main Flow

1. The user copies the private address of one or more streams from the personal schedule.
2. The user adds each address as a subscribed calendar in the application.
3. The application shows the items and refreshes on its own schedule.

## Alternative Flows

None.

## Failure Conditions

Items created in the calendar application never reach the product. Some applications refresh only daily, so changes appear late.

## Postconditions

The application shows the streams read-only.
