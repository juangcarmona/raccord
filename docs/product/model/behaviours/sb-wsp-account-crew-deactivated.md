---
id: SB-WSP-ACCOUNT-CREW-DEACTIVATED
type: structured-behaviour
title: "Deactivation preserves crew history and its association"
status: draft
illustrates:
  - "BR-WSP-ACCOUNT-CREW-ASSOCIATION"
  - "BR-CRW-LOGIN-PERMISSION"
given:
  - "A personal profile is associated with a crew record"
  - "An authorized planner deactivates login access for that crew member"
when: "The login permission is turned off"
then:
  - "The person can no longer sign in through that crew association"
  - "The profile-to-crew association and crew history remain"
  - "Reactivation restores the existing association and does not create a different one"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
---

## Intent

Deactivation removes login access while preserving the historical relationship between the person and crew record.

## Boundaries

Deactivation does not delete or merge either record.
