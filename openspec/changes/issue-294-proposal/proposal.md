<!-- pdac-scope: cited -->

# Proposal

## Why

A fresh installation has a first-administrator API but no browser route to reach it. Expose the approved first-account bootstrap without claiming that later company onboarding is complete.

This implements the accepted `CHG-WSP-FIRST-ADMIN-BROWSER`, integrated through prerequisite PR #450; it does not propose another product decision.

## What Changes

- Route signed-out visitors using server setup status and provide an accessible localized email/password form.
- Preserve first Admin and four predefined roles; serialize competing setup requests and refuse repeats without another account.
- Warn at registration when a password appears in public breach lists, disclosing only a short hash prefix; lookup failure does not prevent setup.
- Recover the earlier partial UI and test it with frontend state tests and a real isolated AppHost/PostgreSQL browser journey.

## Capabilities

### New Capabilities

- `first-run-setup`: initial administrator browser bootstrap, password warning and repeat refusal.

### Modified Capabilities

None. Existing personal-account and cross-surface design requirements remain unchanged.

## Impact

Authentication routing, setup page/translations, setup API response, privacy-preserving breach lookup, installation transaction and browser fixtures/selection. No company/profile, sample-data, preferences, invitations, SSO, financial or canonical product-model changes. Reuse recovered source supplied in owner answers; only the current local branch is available, with no remote epic refs. Product prerequisite PR #450 and predecessor integration are host-supplied provenance, not new claims of forge inspection.

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:bad3a5a4eae32e934a6ace45458a66c14eb33fbcfa02c2c1c22c7c3b922cc0fe" -->
<!-- pdac:cite id="BR-WSP-NEW-USER-DEFAULT-ROLE" digest="sha256:225d068645b0edea78a38030b719e72c188c78fcb75337160dde005db87e6c9c" -->
<!-- pdac:cite id="TERM-PREDEFINED-ROLE" digest="sha256:3e6f316bdfdb4e2c76eede9cbcf2db751b3806adad7f5397c15d27ffdff7fe16" -->
<!-- pdac:cite id="BR-WSP-BREACHED-PASSWORD-CHECK" digest="sha256:8593dbb34d2881761fd788ab1ac34905d59270419aa1c918b0b84603658d6a34" -->
