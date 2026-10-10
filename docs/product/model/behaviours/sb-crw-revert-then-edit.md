---
id: SB-CRW-REVERT-THEN-EDIT
type: structured-behaviour
title: "An approved registration must be reverted to change it"
status: draft
illustrates:
  - "BR-CRW-APPROVED-READ-ONLY"
  - "UC-CRW-REVERT-TIME-REGISTRATION-APPROVAL"
given:
  - "A registration is approved"
when: "The planner reverts it to pending and changes the end time"
then:
  - "The registration is pending with the new end time"
  - "The change is recorded in its history"
uses-terms:
  - "TERM-TIME-REGISTRATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Approved data is stable.

## Boundaries

None.
