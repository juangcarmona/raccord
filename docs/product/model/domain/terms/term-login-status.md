---
id: TERM-LOGIN-STATUS
type: domain-term
title: "Login status"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-USER-ACCOUNT"
  - "TERM-PERSONAL-PROFILE"
  - "TERM-LOCAL-LOGIN"
  - "TERM-ACCOUNT-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/36290993235730.md, .private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36291020702098.md, .private/research/help-center/articles/360014204180.md, .private/research/help-center/articles/15866472895890.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The lifecycle of a user account's ability to sign in.
States:
- No login: the crew member may not log in (login permission off).
- Verification pending: a verification link has been sent and is not yet used.
- Verified: the person confirmed the email address and signs in with it, by password or external sign-in.
Transitions:
| From | To | Trigger |
| --- | --- | --- |
| No login | Verification pending | login permission enabled with an email address (invitation or verification email sent) |
| Verification pending | Verified | the person follows the link and sets a password or chooses an external sign-in |
| Verification pending | Verification pending | a new link is requested after expiry |
| Verified, Verification pending | No login | login permission switched off |
## Distinguish From

Not the account invitation status, which tracks the invitation email to a new crew member; login status covers every way a user may or may not sign in.

## Usage

Shown to administrators to see who still has to verify; determines whether a sign-in attempt is accepted.
