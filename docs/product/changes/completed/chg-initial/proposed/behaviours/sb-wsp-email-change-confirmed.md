---
id: SB-WSP-EMAIL-CHANGE-CONFIRMED
type: structured-behaviour
title: "Confirmed email change updates the crew member"
status: draft
illustrates:
  - "BR-WSP-EMAIL-CHANGE-PROPAGATES"
given:
  - "A person's profile is connected to a crew member"
when: "The person confirms a new address through the verification link"
then:
  - "Sign-in uses the new address from then on"
  - "The connected crew member's email shows the new address"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
provenance:
  source: ".private/research/help-center/articles/15867463701394.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Email changes propagate to company records only after confirmation.

## Boundaries

None.
