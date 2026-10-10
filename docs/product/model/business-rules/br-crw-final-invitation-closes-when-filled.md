---
id: BR-CRW-FINAL-INVITATION-CLOSES-WHEN-FILLED
type: business-rule
title: "Final-planning invitations close when the function is full"
status: draft
applies-to:
  - "UC-CRW-RESPOND-TO-INVITATION"
uses-terms:
  - "TERM-INVITATION-STATUS"
  - "TERM-STAFFING-STATUS"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Once as many crew members as the function requires have accepted a final-planning invitation, the remaining open invitations expire and a later yes is refused with a message that the function is fully planned.

## Rationale

Prevents overstaffing.

## Examples

See SB-CRW-LATE-YES-REFUSED.

## Exceptions

None.
