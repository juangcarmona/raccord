---
id: SB-WSP-ACCOUNT-CREW-UNAUTHORIZED
type: structured-behaviour
title: "Unauthorized crew association requests are refused"
status: draft
illustrates:
  - "BR-WSP-ACCOUNT-CREW-ASSOCIATION"
  - "UC-WSP-LINK-EXISTING-CREW-RECORD"
given:
  - "A user lacks permission to manage crew login access in the workspace"
when: "The user attempts to create or remove a profile-to-crew association"
then:
  - "The product refuses the action without changing either record"
  - "The refusal is audited under the workspace's access-control rules"
  - "No data from another workspace is disclosed"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
---

## Intent

Only an authorized workspace actor can initiate or remove a crew association.

## Boundaries

The product does not disclose whether a profile or crew record exists in another workspace.
