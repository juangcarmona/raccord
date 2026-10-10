---
id: SB-CRW-ACCOUNT-INVITATION-EXPIRED
type: structured-behaviour
title: "Account invitation expiry"
status: draft
illustrates:
  - "BR-CRW-ACCOUNT-INVITATION-EXPIRY"
when: "The person opens the invitation 31 days after it was sent"
then:
  - "The invitation is expired"
  - "After the planner resends it, a new invitation is pending"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Bounded login links.

## Boundaries

None.
