---
id: SB-CRW-CHANGE-NOT-NOTIFIED
type: structured-behaviour
title: "Moving a function after sharing sends nothing"
status: draft
illustrates:
  - "BR-CRW-NO-AUTOMATIC-SCHEDULE-NOTIFICATION"
  - "BR-CRW-CHANGED-SINCE-LAST-SHARE"
given:
  - "A crew member received their schedule containing a function"
when: "The planner moves the function one hour later"
then:
  - "No notification is sent to the crew member"
  - "The crew member is marked as changed when the planner next shares schedules"
uses-terms:
  - "TERM-SCHEDULE-COMMUNICATION"
provenance:
  source: ".private/research/help-center/articles/14488851931026.md, .private/research/help-center/articles/360014672919.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Changes are communicated deliberately.

## Boundaries

None.
