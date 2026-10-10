---
id: "SB-TRB-RESET-DUPLICATE-EMAIL"
type: structured-behaviour
title: "Reset fails while two users share an address"
status: draft
illustrates:
  - "BR-TRB-RESET-NEEDS-UNIQUE-EMAIL"
  - "UC-WSP-MANAGE-PASSWORD"
given:
  - "Two users are registered with office@example-av.com"
when: "One of them requests a password reset with that address"
then:
  - "The reset does not work"
  - "After the other user's address is changed, a new reset request succeeds"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014048499.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shared addresses block self-service recovery.

## Boundaries

None.
