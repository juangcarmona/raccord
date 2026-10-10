---
id: BR-CRW-INVITATION-DISPATCH-BATCHING
type: business-rule
title: "Invitations are sent within 15 minutes and combined"
status: draft
applies-to:
  - "UC-CRW-SEND-PENDING-INVITATIONS"
  - "UC-CRW-INVITE-CREW-FOR-FUNCTION"
uses-terms:
  - "TERM-INVITATION"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Invitations are delivered within 15 minutes of sending; several invitations to the same person within that window are combined into one email with several options.

## Rationale

Fewer, clearer messages for crew.

## Examples

See SB-CRW-INVITATIONS-COMBINED.

## Exceptions

None.
