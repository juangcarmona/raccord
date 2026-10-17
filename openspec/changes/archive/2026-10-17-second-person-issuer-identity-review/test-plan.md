---
pdac-scope: cited
---

# Test plan

<!-- pdac:cite id="FR-ISSUER-001" digest="sha256:1d443dcade4a5af9dce47d75a383a243a298dd37de5a2d1be22bc80d5dfe1712" -->
<!-- pdac:cite id="UC-DEFINE-ISSUER-PROFILE" digest="sha256:fd058a02a9f6f90de16249ea291336c0193cfefce4236e0081a00f6da3223b34" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:f278c6a11a99fe63feadf0105b0ce359e74c71580b9b070a4fbf95c3b34b1604" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="CON-SINGLE-ISSUER" digest="sha256:f41d4ba8da941e762b3ee568fa15b478f1a60443cb2ea905d4826b13cdb1e1cd" -->

## Deterministic tests

- Workspace unit tests: initial setup, pre-issuance update, post-issuance request, reason validation, address-only edits, self-approval, permission denial, approval/rejection, duplicate decision, retained rejected request, and audit payloads.
- Web/API integration tests: permission policies, request/decision response contracts, data persistence across scope, snapshot immutability, and conflict behavior under concurrent decisions.
- Infrastructure tests: migration creates the request table in Workspace schema; invoice existence read port; request state concurrency guard; audit failure rolls back the profile and decision.
- Frontend component tests: reason prompt, pending status, reviewer detail, approve/reject actions, requester cannot decide own request, loading/error states and localized accessible controls.

## End-to-end

Use two separately authenticated seeded test administrators. Create/issue an invoice, submit a legal-identity request as the first administrator, verify the active profile is unchanged, sign in as the second administrator, approve, then verify the new profile is active for a future invoice and the historical snapshot is unchanged. Add the journey to `tests/Raccord.EndToEndTests/per-pr-selection.txt` and capture Playwright artifacts.

## Regression suite

Run the focused Workspace and Web suites, full solution test suite, AppHost selected E2E suite, frontend tests/typecheck/lint, formatting/analyzers, architecture tests, migrations/model check, OpenAPI drift, OpenSpec validation, ProductShape validation and citation verification, plus required CI security and dependency jobs.
