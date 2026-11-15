<!-- pdac-scope: cited -->

- [ ] Add ADR-0028 and shared SMTP configuration/port; document operator settings for Compose, Aspire and Azure without adding credentials.
- [ ] Add the crew account-management permission and current-user capability; test the seeded and custom role behavior.
- [ ] Add invitation and association persistence, unique constraints and migration; verify pending model and concurrent transaction behavior.
- [ ] Implement create/send, resend, accept and reject application flows with hashed one-time tokens, 30-day expiry and auditable outcomes.
- [ ] Add protected API endpoints, generate frontend API types and add English/Spanish planner and acceptance screens.
- [ ] Add unit, integration and frontend tests first for success, rejection, expired/used token, wrong verified email, conflicting association, unauthorized actor, SMTP failure and concurrent acceptance.
- [ ] Add the Workspace/Crew Aspire Playwright journey and evidence attribution in the pinned per-PR selection.
- [ ] Update arc42 security/building-block/deployment and self-hosting docs; create follow-up work for unlinking, deactivation and email-change propagation.
- [ ] Run the full Definition of Done and record any process lesson in the process ledger.

<!-- pdac:cite id="BR-WSP-ACCOUNT-CREW-ASSOCIATION" digest="sha256:1452caa625c12626db7880c8623ed9c726c4f914879a3385a73b2f4a3c9a2966" -->
<!-- pdac:cite id="UC-WSP-LINK-EXISTING-CREW-RECORD" digest="sha256:641960ffc7fd8228156b926012c0ab2edae42b2e0e4fbb4111e1c52e44635462" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-LINKED" digest="sha256:7c138d265a2ebf6645c469fe56442712482a76b5a05cef0f7a3faaf4168bb588" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:2bc8d3c16cae9e821f1aa0297f7025097b9163a6e22fe04599402a904fc59d3d" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->
