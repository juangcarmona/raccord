---
id: UC-WSP-SET-NOTIFICATION-PREFERENCES
type: use-case
title: "Choose notification channels"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-NOTIFICATION-CHANNEL-CHOICE"
  - "BR-CRW-NOTIFICATION-PREFERENCES-SELF-OWNED"
uses-terms:
  - "TERM-NOTIFICATION-PREFERENCE"
  - "TERM-NOTIFICATION-CATEGORY"
provenance:
  source: ".private/research/help-center/articles/360019724320.md, .private/research/help-center/articles/360018840939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The person is notified about what matters through the channels they use.

## Trigger

The person misses notifications or receives too many.

## Preconditions

- The person is signed in.

## Main Flow

1. The person opens notification settings.
2. For each category the person ticks email, mobile web app and/or web.
3. The person saves.

## Alternative Flows

- None.

## Failure Conditions

- Phone notifications still do not arrive: the person checks the browser and phone permissions.

## Postconditions

- Future notifications follow the chosen channels.
