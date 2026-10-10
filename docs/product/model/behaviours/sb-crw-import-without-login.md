---
id: SB-CRW-IMPORT-WITHOUT-LOGIN
type: structured-behaviour
title: "Importing crew without sending invitations"
status: draft
illustrates:
  - "BR-CRW-ACCOUNT-INVITATION-ON-CREATION"
  - "UC-CRW-IMPORT-CREW-MEMBERS"
when: "The planner imports a file whose login permission column is 0 for all rows"
then:
  - "The crew members are created"
  - "No account invitation email is sent"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014366299.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Quiet set-up of the installation.

## Boundaries

None.
