---
id: SB-WSP-ACCOUNT-CREW-EMAIL-CHANGED
type: structured-behaviour
title: "A verified email change preserves the crew association"
status: draft
illustrates:
  - "BR-WSP-ACCOUNT-CREW-ASSOCIATION"
  - "BR-WSP-EMAIL-CHANGE-PROPAGATES"
given:
  - "A personal profile is already associated with a crew record"
  - "The person changes the profile email and verifies the new address"
when: "The verified email change is applied"
then:
  - "The new email propagates to the linked crew record under the existing rule"
  - "The profile-to-crew association remains unchanged"
  - "No other profile or crew record is linked based on an email match"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
---

## Intent

Changing a sign-in email updates the connected contact details without changing who the profile represents.

## Boundaries

Email equality alone never creates or moves an association.
