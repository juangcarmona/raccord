---
id: SB-INT-PERMISSION-AFTER-RELOGIN
type: structured-behaviour
title: "Granted permission visible after signing in again"
status: draft
illustrates:
  - "BR-INT-ROLE-CHANGE-NEEDS-NEW-SESSION"
given:
  - "the permission was granted while the user was signed in"
  - "a refresh did not show the option"
when: "the user signs out and in again"
then:
  - "the newly granted option is shown"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/35485155508370.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Session-bound permissions.

## Boundaries

None.
