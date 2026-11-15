<!-- pdac-scope: cited -->

## Why

The current Product Definition distinguishes a personal profile from a crew record and defines invitation and email behavior, but it does not define how an existing profile becomes safely associated with an existing crew record. A wrong association can expose personal and work history across identities.

## What Changes

- Propose a Product Change defining account-to-crew association, confirmation, conflict handling, unlinking, audit and deactivation behavior.
- Use a planner's invitation to one selected crew record plus the authenticated person's explicit acceptance as sufficient confirmation; no second planner approval is required.
- Keep email equality insufficient as proof and leave the sufficiency of the proposed confirmation and need for second-planner approval as explicit Owner questions.
- Apply the approved Product Change in this policy change and track runtime implementation in the separate #369 work item.

## Capabilities

### New Capabilities

- `account-crew-association`: Define the policy for linking and unlinking a personal profile and a crew record while preserving their separate identities.

### Modified Capabilities

- None.

## Impact

Product Definition artifacts in Workspace and Crew; a future account-to-crew runtime flow; audit and access-control expectations. No runtime code, API, data migration, authentication provider, credential, production data or deployment change is included.

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
