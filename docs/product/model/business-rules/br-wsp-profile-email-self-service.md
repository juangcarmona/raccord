---
id: BR-WSP-PROFILE-EMAIL-SELF-SERVICE
type: business-rule
title: "Only the person changes a profile's email address"
status: draft
applies-to:
  - "UC-WSP-CHANGE-PROFILE-EMAIL"
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-USER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360017645240.md, .private/research/help-center/articles/360014997360.md, .private/research/help-center/articles/15867463701394.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Once a user has verified a login email, nobody else can change that account's login email; only the person can change it from their profile, after which the new address must be verified.

## Rationale

The verified email is the person's sign-in identity.

## Examples

- An administrator editing a crew member with a verified login cannot alter the login email field.

## Exceptions

- Accounts without a verified login can have their email corrected by an administrator.
