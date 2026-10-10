---
id: FR-WSP-021
type: functional-requirement
title: "Notification preferences and delivery"
status: draft
derived-from:
  - "UC-WSP-SET-NOTIFICATION-PREFERENCES"
  - "UC-WSP-RECEIVE-NOTIFICATION"
  - "BR-WSP-NOTIFICATION-CHANNEL-CHOICE"
verification:
  - scenario: "A user who enabled only web notifications for tasks receives a web notification and no email when a task is assigned"
uses-terms:
  - "TERM-NOTIFICATION"
  - "TERM-NOTIFICATION-CATEGORY"
provenance:
  source: ".private/research/help-center/articles/360019724320.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST notify users of the documented event categories through the channels each user enabled per category.

## Rationale

People need to know about relevant changes.
