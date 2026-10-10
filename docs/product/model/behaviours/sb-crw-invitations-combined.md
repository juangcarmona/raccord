---
id: SB-CRW-INVITATIONS-COMBINED
type: structured-behaviour
title: "Invitations within 15 minutes are combined"
status: draft
illustrates:
  - "BR-CRW-INVITATION-DISPATCH-BATCHING"
when: "The planner sends a second invitation to the same crew member five minutes after the first"
then:
  - "The crew member receives one email listing both options within 15 minutes"
uses-terms:
  - "TERM-INVITATION"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Fewer messages.

## Boundaries

None.
