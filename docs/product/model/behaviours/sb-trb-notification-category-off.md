---
id: "SB-TRB-NOTIFICATION-CATEGORY-OFF"
type: structured-behaviour
title: "Missing notifications explained by preferences"
status: draft
illustrates:
  - "BR-WSP-NOTIFICATION-CHANNEL-CHOICE"
  - "UC-WSP-SET-NOTIFICATION-PREFERENCES"
given:
  - "Crew member Lisa has only the web channel ticked for the invitation category"
when: "A planner sends Lisa an invitation"
then:
  - "Lisa receives the notification only as a web notification"
  - "No e-mail and no mobile notification is sent to her"
uses-terms:
  - "TERM-NOTIFICATION-PREFERENCE"
  - "TERM-NOTIFICATION-CATEGORY"
provenance:
  source: ".private/research/help-center/articles/360014992520.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Absence of notifications is usually a preference setting.

## Boundaries

None.
