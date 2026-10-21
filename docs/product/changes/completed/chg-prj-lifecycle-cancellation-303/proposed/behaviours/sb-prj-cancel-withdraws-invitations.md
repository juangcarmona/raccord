---
id: SB-PRJ-CANCEL-WITHDRAWS-INVITATIONS
type: structured-behaviour
title: "Withdrawing invitations on cancellation"
status: active
illustrates:
  - "BR-PRJ-CANCELLATION-ACTIONS-APPLICABLE"
  - "UC-PRJ-CANCEL-PROJECT"
given:
  - "A project has one sent and one unsent crew invitation"
when: "The coordinator cancels it choosing to withdraw invitations"
then:
  - "The sent invitation expires and can no longer be answered"
  - "The unsent invitation is deleted"
  - "Functions remain on the project"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/28083930405778.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Defines the effect of the withdraw action.

## Boundaries

None.
