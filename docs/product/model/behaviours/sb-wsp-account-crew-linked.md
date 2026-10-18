---
id: SB-WSP-ACCOUNT-CREW-LINKED
type: structured-behaviour
title: "A person confirms a planner-selected crew record"
status: draft
illustrates:
  - "BR-WSP-ACCOUNT-CREW-ASSOCIATION"
  - "UC-WSP-LINK-EXISTING-CREW-RECORD"
given:
  - "An authorized planner selects an existing crew record that is not linked to another profile"
  - "The crew record has a current email address and an unexpired invitation is sent"
  - "The person has an authenticated personal profile in the workspace"
when: "The person reviews the selected crew record and explicitly accepts the invitation"
then:
  - "The personal profile is linked to the selected crew record"
  - "The records remain distinct and retain their existing history"
  - "The link outcome records the actor, time, workspace and affected record identifiers"
  - "The person receives only the access allowed for that crew member"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
  - "TERM-ACCOUNT-INVITATION-STATUS"
---

## Intent

The planner's explicit record selection and the authenticated person's acceptance establish the proposed association.

## Boundaries

The product does not establish identity from email equality alone and does not merge the profile with the crew record.
