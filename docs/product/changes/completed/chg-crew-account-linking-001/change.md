---
id: CHG-CREW-ACCOUNT-LINKING-001
type: product-change
title: Define verified association between crew records and personal profiles
status: applied
base-revision: "25dddc0cdb6f5453a78086daaca059512fbbf203"
operations:
    add:
        - UC-WSP-LINK-EXISTING-CREW-RECORD
        - BR-WSP-ACCOUNT-CREW-ASSOCIATION
        - SB-WSP-ACCOUNT-CREW-LINKED
        - SB-WSP-ACCOUNT-CREW-AMBIGUOUS
        - SB-WSP-ACCOUNT-CREW-UNAUTHORIZED
        - SB-WSP-ACCOUNT-CREW-UNLINKED
        - SB-WSP-ACCOUNT-CREW-EMAIL-CHANGED
        - SB-WSP-ACCOUNT-CREW-DEACTIVATED
    modify: []
    remove: []
---

## Problem

The current Product Definition treats a personal profile and a crew record as distinct concepts and describes invitations and verified-email uniqueness, but does not define how an existing profile and an existing crew record are associated, corrected or unlinked.

## Intended Product Outcome

The Product Definition describes a safe, auditable process for associating a personal profile with the correct existing crew record, keeps those records distinct, and defines outcomes for ambiguous matches, conflicting links, email changes, unlinking and deactivation.

The approved policy is two-sided confirmation: an authorized planner initiates an invitation scoped to one existing crew record, and the authenticated profile holder explicitly accepts it. This is sufficient confirmation; no second planner approval is required. Email equality alone never creates or proves the association. The invitation goes to the crew record's current email, and an existing profile must authenticate before accepting.

## Rationale

A crew record carries planning and work history, while a personal profile grants access. Linking them based only on an email match risks exposing another person's records. Requiring a planner to choose the specific record and the profile holder to accept provides a reviewable confirmation without collecting identity documents. The initiating planner's existing permission is sufficient; a second approval would add friction without changing the person's explicit acceptance.

## Affected Product Areas

- Workspace profile identity and crew-record associations.
- Crew login invitations and planner permissions.
- Profile email changes, crew email propagation, unlinking and deactivation.
- Audit records and workspace data isolation.

## Open Questions

None. The approved policy uses the planner's record-specific invitation plus the authenticated profile holder's explicit acceptance as sufficient confirmation. A second planner approval is not required.

## Product Acceptance

- The approved decision records that record-specific planner invitation plus authenticated acceptance is sufficient confirmation and does not require a second planner approval.
- The accepted Product Definition states that email equality alone never associates a profile with a crew record.
- Only an authorized actor may initiate or remove a link; the person must confirm the proposed association before it takes effect.
- Ambiguous, conflicting, unauthorized, rejected, changed-email, unlinked and deactivated cases have explicit outcomes and auditable events.
- The change preserves workspace isolation, crew history, profile uniqueness and invitation expiry.
- ProductShape validates the overlay with no errors, and structured behaviours cover success and the key failure and lifecycle cases.

## Out of Scope

This change does not implement runtime workflows, merge or delete profiles or crew records, change authentication providers, collect identity documents, modify production data, make legal-compliance claims or decide whether a real person has proven their identity.
