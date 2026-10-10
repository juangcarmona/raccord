---
id: FR-WSP-007
type: functional-requirement
title: "Self-service profile email change"
status: draft
derived-from:
  - "UC-WSP-CHANGE-PROFILE-EMAIL"
  - "BR-WSP-PROFILE-EMAIL-SELF-SERVICE"
  - "BR-WSP-EMAIL-CHANGE-PROPAGATES"
verification:
  - scenario-ref: "SB-WSP-CONNECTED-EMAIL-LOCKED"
  - scenario-ref: "SB-WSP-EMAIL-CHANGE-CONFIRMED"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
provenance:
  source: ".private/research/help-center/articles/15867463701394.md, .private/research/help-center/articles/360017645240.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let only the person change their profile email, confirm it by verification link, and propagate it to the connected crew member.

## Rationale

Identity belongs to the person.
