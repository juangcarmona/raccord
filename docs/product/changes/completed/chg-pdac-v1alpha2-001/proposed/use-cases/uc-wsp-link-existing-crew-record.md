---
id: UC-WSP-LINK-EXISTING-CREW-RECORD
type: use-case
title: "Link a personal profile to an existing crew record"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-ACCOUNT-CREW-ASSOCIATION"
  - "BR-CRW-ACCOUNT-INVITATION-EXPIRY"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
  - "TERM-ACCOUNT-INVITATION-STATUS"
---

## Goal

The correct person can use their personal profile to access their existing crew record without merging the two records.

## Trigger

A crew planner needs to grant login access to an existing crew member who already has a personal profile, or the person needs to correct a missing association.

## Preconditions

- The planner is authenticated and has permission to manage crew login access in the workspace.
- The crew record and personal profile are in the same workspace context and remain separate records.

## Main Flow

1. The planner selects one existing crew record and requests a link invitation.
2. The product sends a time-limited invitation to the email currently recorded for that crew member.
3. The person authenticates to their personal profile and reviews which crew record is being linked.
4. The person explicitly accepts the invitation.
5. The product verifies that neither record is linked elsewhere and creates the association.
6. The product records an auditable outcome and grants only the access permitted for that crew member.

## Alternative Flows

- If the person has no profile, the existing crew invitation and account-creation flow applies.
- If no record, multiple plausible records or a conflicting link is found, the product refuses to guess and asks the planner to resolve it.
- The person may reject the invitation; no association is created.

## Failure Conditions

- The planner lacks permission: the request is refused without changing either record.
- The invitation has expired: the planner must resend it under the existing expiry rule.
- The crew record is already associated with another profile, or the profile is already associated with another crew record in the workspace: the request is refused for planner resolution.
- The invitation email does not match the selected crew record's current email: the request is not accepted; the planner must correct the record through the established process.

## Postconditions

- After acceptance, one personal profile is associated with one crew record, and both records and their history remain distinct.
- Rejected or refused attempts do not change the association; their outcomes are audited.
