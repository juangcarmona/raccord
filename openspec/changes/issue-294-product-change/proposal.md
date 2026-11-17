<!-- pdac-scope: cited -->

# First workspace administrator through the browser

## Why

Issue #294 asks for a fresh installation to expose first-account creation in the browser, with one Admin and refusal of repeat setup. The accepted setup flow combines first-account creation with later company details and sample-data onboarding, while the default-role rule has no first-account exception.

## Requested Product Intent

A visitor to an installation with no user reaches accessible first-run setup and creates the first administrator with email and password. The four predefined roles are available. Once any user exists, first-account setup is unavailable and repeat attempts create no account. Later users retain the Crew default. Subsequent company onboarding and its requirements remain distinct.

## Impacted Product Areas

- UC-WSP-CREATE-WORKSPACE: distinguish the bootstrap from subsequent company setup.
<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:e5ee7da50f5c081b1c257461484a9e6eecfbce0f108a5abd2d388c14c8e4f16a" -->
- BR-WSP-NEW-USER-DEFAULT-ROLE: express the first-account exception.
<!-- pdac:cite id="BR-WSP-NEW-USER-DEFAULT-ROLE" digest="sha256:2623af3a725e6d94b3ac275e0812f882e5696359997ae10e663ab01ae0fe8b4d" -->
- FR-WSP-013: retain the normal default without contradicting the exception.
<!-- pdac:cite id="FR-WSP-013" digest="sha256:71858127bfa1e12d421b7f6568afb1b982b6ee763d53bed2feb94e3104f6e5bc" -->
- TERM-PREDEFINED-ROLE: clarify the same exception without changing the four roles.
<!-- pdac:cite id="TERM-PREDEFINED-ROLE" digest="sha256:40c8223391cb84a39ccfa1047f4cec3e38063e401caf61b52a31a2d14d69e7c5" -->
- JRN-WSP-SET-UP-WORKSPACE: the surrounding journey still includes subsequent onboarding.
<!-- pdac:cite id="JRN-WSP-SET-UP-WORKSPACE" digest="sha256:e5a03909059bff96da07389ff0214635e74d4085fa6aab2b46c48cba4b88ccc9" -->
- FR-WSP-025: sample data and removal remain required for the later installation setup, not newly implemented by this bootstrap.
<!-- pdac:cite id="FR-WSP-025" digest="sha256:700640fa49deb96f6923e40e642c68d62503d6e8db712e16c49aad40fbcd64c7" -->

## Neighbours Checked and Left Out

JRN-WSP-SET-UP-WORKSPACE already sequences the broader onboarding and remains unchanged. FR-WSP-025 retains the sample-data obligation. UC-INSTALL-RACCORD retains installation prerequisites and issuer-ready postcondition; UC-DEFINE-ISSUER-PROFILE retains financial and legal safeguards. Company details, language, time zone, sign-in alternatives and password warnings retain their existing meaning outside the first-account step. BC-WORKSPACE and the one-role rule remain unchanged.

## Clarification Report

- UC-WSP-CREATE-WORKSPACE → JRN-WSP-SET-UP-WORKSPACE, steps relationship, dependency: checked and unaffected; its broader journey still follows setup.
- UC-WSP-CREATE-WORKSPACE → FR-WSP-025, derived-from, dependency: checked and unaffected; bootstrap does not remove later sample-data requirements.
- UC-WSP-CREATE-WORKSPACE → password and time-zone rules, governed-by, governance: checked and unaffected; preserve warnings and later baseline behavior.
- BR-WSP-NEW-USER-DEFAULT-ROLE → FR-WSP-013, derived-from, dependency: affected because its unconditional wording would contradict the first-account exception.
- BR-WSP-NEW-USER-DEFAULT-ROLE → TERM-PREDEFINED-ROLE, uses-terms, dependency: affected because its unconditional Crew wording needs the same exception. UC-WSP-ASSIGN-ROLE is checked and unaffected for subsequently added users.
- TERM-PREDEFINED-ROLE → BC-WORKSPACE, defined-in, dependency: checked and unaffected; the role definitions and permissions stay intact. The supplied BC report is depth one, not an exhaustive native impact report.
- FR-WSP-013 → role assignment and review use cases, derived-from, dependency: checked and unaffected; ordinary role administration is unchanged.
- UC-INSTALL-RACCORD has no provider-reported graph link to bootstrap; its operator-supplied report and issuer-ready postcondition were checked separately.

## Refinement pass

Queue: bounded first-account bootstrap and role exception. Excluded: company-profile UI, sample data, language and time-zone preferences, invitations, SSO and production actions. The verified owner reconciliation says: "first-account creation succeeds before company name, company language and phone capture. Those details are outside this first-account screen and must not be introduced as new prerequisites." It also says: "Reconcile the first-Admin exception to BR-WSP-NEW-USER-DEFAULT-ROLE while preserving Crew for subsequent users." No material product decisions remain unresolved for this bounded candidate; validation and any product decision are separate. The supplied installation report and depth-one workspace graph are supplements, not native completion proofs.

## Open Questions for the Product Owner

None.
