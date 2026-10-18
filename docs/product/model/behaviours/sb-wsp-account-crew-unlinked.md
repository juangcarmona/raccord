---
id: SB-WSP-ACCOUNT-CREW-UNLINKED
type: structured-behaviour
title: "An authorized planner removes a crew association"
status: draft
illustrates:
  - "BR-WSP-ACCOUNT-CREW-ASSOCIATION"
  - "UC-WSP-LINK-EXISTING-CREW-RECORD"
given:
  - "A personal profile is associated with a crew record"
  - "An authorized planner has a reason to remove the association"
when: "The planner removes the association and records the reason"
then:
  - "The association is removed and the outcome records actor, time, workspace, affected record identifiers and reason"
  - "The personal profile and crew record remain separate and retain their histories"
  - "The former association no longer grants access to the crew record"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
---

## Intent

An association can be corrected without deleting either identity or its history.

## Boundaries

Removing the association does not merge records or erase audit history.
