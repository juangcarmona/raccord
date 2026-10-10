---
id: SB-WSP-BREACHED-PASSWORD
type: structured-behaviour
title: "Leaked password triggers a warning only"
status: draft
illustrates:
  - "BR-WSP-BREACHED-PASSWORD-CHECK"
when: "The person signs in with a password found in public breach lists"
then:
  - "The product warns that the password appeared in an external breach"
  - "The person is advised to change it"
  - "The password itself is never sent to the checking service"
uses-terms:
  - "TERM-BREACHED-PASSWORD-WARNING"
provenance:
  source: ".private/research/help-center/articles/31560381502226.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Clarifies that the warning is advisory and privacy preserving.

## Boundaries

Does not assert that sign-in is blocked.
