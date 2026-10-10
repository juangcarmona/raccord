---
id: UC-WSP-SIGN-IN
type: use-case
title: "Sign in"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-BREACHED-PASSWORD-CHECK"
  - "BR-WSP-SSO-COMPLIANCE"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-EXTERNAL-SIGN-IN"
provenance:
  source: ".private/research/help-center/articles/360014204180.md, .private/research/help-center/articles/360014997360.md, .private/research/help-center/articles/360018767240.md"
  confidence: high
  recovered-from: documentation
---
## Goal
The person reaches the company's installation with their own identity.
## Trigger
The person opens the installation's sign-in address.
## Preconditions
- The person has a user account with login allowed and a verified email address.
## Main Flow
1. The person signs in with email and password or, where offered, with a Microsoft or Google account.
2. The product opens the application with the permissions of the person's role.
## Alternative Flows
- The password appears in breach lists: the product warns and offers to change it, then continues.
## Failure Conditions
- Wrong credentials: sign-in is refused and a password reset can be requested.
- The user's role enforces single sign-on and the person is not compliant: sign-in is refused.
## Postconditions
- The person works in the application with their role's permissions.
