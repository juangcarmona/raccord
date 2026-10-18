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
<!-- pdac:cite id="UC-WSP-JOIN-WORKSPACE" digest="sha256:1e77d6ae23bb9ffd002ade2efb6ec362fc71cb1f10a3de666b288d32bed71aa7" -->
<!-- pdac:cite id="UC-WSP-CHANGE-PROFILE-EMAIL" digest="sha256:0fc2f64f44eb70c14f2dfb4bfc0e304a65079d86117f447c495355657084830a" -->
<!-- pdac:cite id="UC-CRW-MANAGE-CREW-LOGIN-ACCESS" digest="sha256:426f879ee3742eede9023e4cbf461c614f50ac8eb9eceec95147d29a7fc59dc7" -->
<!-- pdac:cite id="BR-WSP-ONE-PROFILE-PER-EMAIL" digest="sha256:93bc9f499b51fa1065d951dcfa00afb28c481a787f000765bf94411f973edeab" -->
<!-- pdac:cite id="BR-WSP-VERIFY-UNIQUE-IN-WORKSPACE" digest="sha256:e1f7f1b23072a7df82cbb1627611fd1848aaa7b28d6b68fb4143b569cc91c0b0" -->
<!-- pdac:cite id="BR-WSP-EMAIL-CHANGE-PROPAGATES" digest="sha256:df4a7ae92988bc3e0bbcd8da69dc0d2f66363a7e646b183d534117ac707c3b91" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:0b7b348c641e79d8d97ff46b53c5f869b08f5d042d360429810ee6cc67f3c5c1" -->
<!-- pdac:cite id="BR-CRW-PROFILE-OWNERSHIP" digest="sha256:8b7c288163c50c7481acce8b6d361ec6ff7cd1e5beea0018ee097e71e7fb1e5f" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->

<!-- pdac:cite id="BR-WSP-ACCOUNT-CREW-ASSOCIATION" digest="sha256:7b1c7b7971ae29810b6f4999edf899302fd7fa738a8bc2b7cbf2d6d8ad70ee6f" -->
<!-- pdac:cite id="UC-WSP-LINK-EXISTING-CREW-RECORD" digest="sha256:641960ffc7fd8228156b926012c0ab2edae42b2e0e4fbb4111e1c52e44635462" -->
