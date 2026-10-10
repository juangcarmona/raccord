---
id: BR-WSP-NOTIFICATION-CHANNEL-CHOICE
type: business-rule
title: "Notification channels are chosen per category"
status: draft
applies-to:
  - "UC-WSP-SET-NOTIFICATION-PREFERENCES"
  - "UC-WSP-RECEIVE-NOTIFICATION"
uses-terms:
  - "TERM-NOTIFICATION-CATEGORY"
  - "TERM-NOTIFICATION-PREFERENCE"
provenance:
  source: ".private/research/help-center/articles/360019724320.md, .private/research/help-center/articles/360018840939.md"
  confidence: high
  recovered-from: documentation
---

## Rule

For each notification category a user chooses any combination of email, mobile web app and web notifications; the product sends each notification through exactly the chosen channels.

## Rationale

People work in different places.

## Examples

- A crew member receives invitation notifications on the phone and by email but task notifications only on the web.

## Exceptions

None.
