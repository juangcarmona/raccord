---
id: BR-WSP-ACCOUNT-CREW-ASSOCIATION
type: business-rule
title: "Personal profiles link to crew records only after confirmation"
status: draft
applies-to:
  - "UC-WSP-LINK-EXISTING-CREW-RECORD"
  - "UC-WSP-JOIN-WORKSPACE"
  - "UC-WSP-CHANGE-PROFILE-EMAIL"
  - "UC-CRW-MANAGE-CREW-LOGIN-ACCESS"
uses-terms:
  - "TERM-USER-ACCOUNT"
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
  - "TERM-ACCOUNT-INVITATION-STATUS"
---

## Rule

A personal profile and a crew record remain distinct records. A link is created only after an authorized planner selects one existing crew record and the authenticated personal-profile holder explicitly accepts an invitation scoped to that record. Matching email addresses alone never create or prove a link. The invitation is delivered to the current email on the selected crew record and follows the existing invitation expiry rule. One crew record may be linked to at most one personal profile, and one profile may be linked to at most one crew record within a workspace.

If no crew record matches the invitation, more than one plausible record exists, or the selected record is already linked to another profile, the link is refused and requires planner resolution; the product must not guess, merge records or expose another profile's data. An unauthorized request is refused without changing either record. A link removal requires an authorized planner action and a reason; it removes access through that association but preserves both records and their history. Successful, rejected and removed link attempts record the actor, time, workspace and affected record identifiers, outcome and reason without storing credentials or identity documents.

A verified profile-email change updates the linked crew record under BR-WSP-EMAIL-CHANGE-PROPAGATES and does not change the association. A crew member whose login access is deactivated can no longer use the association to sign in; the link and historical crew data remain. Reactivation does not create or change a link.

## Rationale

A planner's selection identifies the intended crew record, while acceptance by the authenticated profile holder confirms that the account holder agrees to the association. Neither email equality nor a planner's guess is enough to establish identity.

## Examples

A planner selects one crew record and sends its invitation; the already authenticated person accepts it, and the product links those two records. If two records could match, no association is made until a planner resolves the ambiguity.

## Exceptions

None.
