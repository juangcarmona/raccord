<!-- pdac-scope: cited -->

## Context

An account represents a person's authentication identity. A crew record represents their workspace role, planning history and crew data. Existing invitation behavior supports a new profile for a crew record, while verified-email rules ensure profile uniqueness. Those behaviors do not establish that an existing profile and an existing crew record represent the same person.

## Goals / Non-Goals

**Goals:** Define a tenant-scoped, auditable rule for associating existing records; preserve each record and its history; refuse ambiguous or conflicting matches; state how email changes, unlinking and deactivation behave.

**Non-Goals:** Implement the flow, decide a person's identity from email equality, collect identity documents, merge records, alter authentication, make compliance claims or alter production data.

## Decisions

- Add a Workspace use case and business rule with structured scenarios grouped under the Workspace bounded context.
- Use two-sided confirmation: an authorized planner selects one existing crew record and sends a time-limited invitation to its current email; the authenticated profile holder reviews and accepts that specific association. This is sufficient confirmation; the initiating planner's existing permission is sufficient and a second planner approval is not required.
- Treat the two-sided confirmation decision as approved by the Owner for this change. Email equality alone is insufficient proof.
- Refuse to create a link when there is no plausible record, multiple plausible records, duplicate-email ambiguity, or either record is already linked elsewhere. Require planner resolution without revealing data from another workspace.
- Require authorized, reasoned unlinking; retain both records and audit history.
- Keep profile email propagation governed by BR-WSP-EMAIL-CHANGE-PROPAGATES without changing the association. Deactivation blocks login but preserves the existing link and crew history.
- Apply the approved Product Change and track subsequent code, API, persistence and browser journey in a separate implementation issue.

## Risks / Trade-offs

Two-sided confirmation avoids identity documents and makes the affected person an active participant. It relies on the inviter selecting the intended record and the authenticated profile holder reviewing the invitation; unusual identity disputes still require manual resolution. A second planner approval is intentionally omitted because it adds operational friction without replacing the person's confirmation.

## Verification Plan

- Validate the Product Change overlay with ProductShape before apply; resolve all diagnostics.
- Review the proposed Workspace and Crew artifacts and verify every association, rejection, unlink and lifecycle scenario.
- Verify that existing email uniqueness, invitation expiry, crew login permission, profile ownership, tenant isolation and audit expectations remain consistent.
- After apply, open a separate runtime issue. Its verification must include authorization, tenant isolation, audit events, ambiguity, conflicts, changed email, unlinking and deactivation, plus a browser journey selected for CI.

<!-- pdac:cite id="TERM-USER-ACCOUNT" digest="sha256:e35b221d981e03d6c106ca25ff26ff507b9a616cd8e01ca80a9f6ee598b44d9c" -->
<!-- pdac:cite id="TERM-PERSONAL-PROFILE" digest="sha256:9e904c99957e8115e03adbe906f242aae13fa65eb1dea0e1a4f7bf1667d9675c" -->
<!-- pdac:cite id="TERM-CREW-MEMBER" digest="sha256:be1b9db1fba459e769e76964f44ebf5b4dc94b31f75ee257b060b6f2604b492e" -->
<!-- pdac:cite id="UC-WSP-JOIN-WORKSPACE" digest="sha256:ae1594869df4c3b607cafdc77c74c5d57b2de52e8dd99eae6e2205d208c29006" -->
<!-- pdac:cite id="UC-WSP-CHANGE-PROFILE-EMAIL" digest="sha256:71ae8f6ebb91255f46137e0fe07ff846148e44b461c645037309d0a5522a09f1" -->
<!-- pdac:cite id="UC-CRW-MANAGE-CREW-LOGIN-ACCESS" digest="sha256:6dbdc959fe855ef60bc10f8927da76ec056e78a6af72464f45266f69daba51dc" -->
<!-- pdac:cite id="BR-WSP-ONE-PROFILE-PER-EMAIL" digest="sha256:c7a7278ed5c748d335a0c36c81d74ae0c34f6820aba18c7a8b2a338c006e82eb" -->
<!-- pdac:cite id="BR-WSP-VERIFY-UNIQUE-IN-WORKSPACE" digest="sha256:e1f7f1b23072a7df82cbb1627611fd1848aaa7b28d6b68fb4143b569cc91c0b0" -->
<!-- pdac:cite id="BR-WSP-EMAIL-CHANGE-PROPAGATES" digest="sha256:be0bcbb9c0d4119d48695fef7d096019c945ae4999809937ac59c131c554597b" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:2bc8d3c16cae9e821f1aa0297f7025097b9163a6e22fe04599402a904fc59d3d" -->
<!-- pdac:cite id="BR-CRW-PROFILE-OWNERSHIP" digest="sha256:f4202845b7649678220c75b4f144a4be80467e72a5eec42ccc5a1e16ec52ea3b" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->

<!-- pdac:cite id="BR-WSP-ACCOUNT-CREW-ASSOCIATION" digest="sha256:1452caa625c12626db7880c8623ed9c726c4f914879a3385a73b2f4a3c9a2966" -->
<!-- pdac:cite id="UC-WSP-LINK-EXISTING-CREW-RECORD" digest="sha256:641960ffc7fd8228156b926012c0ab2edae42b2e0e4fbb4111e1c52e44635462" -->
