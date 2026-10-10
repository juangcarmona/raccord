---
id: "SB-TRB-PASSWORD-CHANGE-WRONG-CURRENT"
type: structured-behaviour
title: "Wrong current password blocks the change"
status: draft
illustrates:
  - "BR-TRB-PASSWORD-CHANGE-NEEDS-CURRENT"
given:
  - "The person is signed in with a local login"
when: "The person enters a wrong current password and a new password and saves"
then:
  - "The password is not changed"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014048499.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Current password is a precondition.

## Boundaries

None.
