<!-- pdac-scope: cited -->

## Context

The product has distinct crew records and Identity profiles but no association, no Crew account-management permission and no email transport. ADR-0012 defines one organization per installation. The accepted product policy requires an authorized planner to select a specific crew record and the authenticated profile holder to accept; matching email alone is not identity proof.

## Goals / Non-Goals

**Goals:** Deliver a secure, expiring invitation; verify the profile holder against the current crew email; create a one-to-one association atomically; audit outcomes; keep SMTP replaceable and safe to test; provide planner and acceptance browser flows.

**Non-Goals:** Add multi-tenancy; merge records; remove an established association; deactivate/reactivate accounts; propagate later profile-email changes; add a third-party email account or read production credentials.

## Decisions

- Add the Workspace permission `crew:accounts.manage`. Grant it to the seeded Admin and Office roles and allow custom roles to grant it; do not grant it to the Crew role. Protect invite-management endpoints with this permission.
- Persist the profile-to-crew association in the Workspace schema with unique foreign keys to the existing Identity user and Crew member, matching ADR-0012's single-organization boundary.
- Persist invitations with target email, selected crew id, issuer, issue/expiry timestamps, status and SHA-256 digest of a cryptographically random token. Do not persist or log the raw token. Bind acceptance to an authenticated user whose verified email equals the invitation's target address.
- Apply invitation consumption and association creation in one database transaction. Unique constraints are the final concurrency guard; a losing concurrent acceptance receives a conflict and creates no duplicate association.
- Keep invitation lifecycle separate from the existing project-function invitation model. The 30-day expiry rule is shared, but the invitation has distinct purpose and authorization.
- Add an application-owned email sender port and an SMTP adapter using MailKit 4.17.0, as recorded in ADR-0028. Missing configuration leaves application startup unaffected; sending fails visibly and safely so a planner can retry. Tests use a recording sender.
- Add a planner screen under Workspace crew access and an authenticated invitation acceptance screen. Keep unauthenticated visitors on the sign-in path with the intended acceptance route preserved.
- Record actor, timestamp, workspace/install scope, affected ids, outcome and reason through the existing audit path. Exclude token, password and SMTP secrets.
- Keep runtime evidence grouped by the Workspace and Crew bounded contexts and register the journey in the pinned per-PR selection.

## Risks / Trade-offs

A forwarded invitation must not let a different profile claim the record; requiring a verified email match and authenticated acceptance reduces that risk. SMTP can be misconfigured or unavailable, so the invitation must remain retryable without exposing its token. The current installation has one organization, so no WorkspaceId column is introduced.

## Verification Plan

- API/application tests cover authorization, expiry, one-time token use, email mismatch, ambiguity, duplicate links and concurrent acceptance against PostgreSQL.
- SMTP adapter tests exercise configuration, TLS selection and error mapping with a local capture service or test double; no real destination is used.
- Frontend tests cover permission denial, invite submission, sign-in return path, acceptance/rejection and es/en strings.
- Aspire E2E captures the planner invitation and crew-member acceptance journey with result evidence attributed to Workspace and Crew.
- Run contract generation, OpenSpec, ProductShape/citation checks, format, analyzers, all tests, security and CI mirrors.

<!-- pdac:cite id="BR-WSP-ACCOUNT-CREW-ASSOCIATION" digest="sha256:7b1c7b7971ae29810b6f4999edf899302fd7fa738a8bc2b7cbf2d6d8ad70ee6f" -->
<!-- pdac:cite id="UC-WSP-LINK-EXISTING-CREW-RECORD" digest="sha256:641960ffc7fd8228156b926012c0ab2edae42b2e0e4fbb4111e1c52e44635462" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-LINKED" digest="sha256:7c138d265a2ebf6645c469fe56442712482a76b5a05cef0f7a3faaf4168bb588" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-AMBIGUOUS" digest="sha256:9ecc890588c04cc82326869f03ced14090910fee52a445c756a1274717b5071d" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:0b7b348c641e79d8d97ff46b53c5f869b08f5d042d360429810ee6cc67f3c5c1" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->
