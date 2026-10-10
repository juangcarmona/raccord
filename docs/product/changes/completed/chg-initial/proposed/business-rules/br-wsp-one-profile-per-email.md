---
id: BR-WSP-ONE-PROFILE-PER-EMAIL
type: business-rule
title: "A verified email address identifies exactly one user"
status: draft
applies-to:
  - "BC-WORKSPACE"
  - "UC-WSP-CHANGE-PROFILE-EMAIL"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
provenance:
  source: ".private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36290993235730.md, .private/research/help-center/articles/15867463701394.md, .private/research/help-center/articles/36291020702098.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A verified email address belongs to at most one personal profile, and no two user accounts may use the same email address.

## Rationale

The email address is the sign-in identity and the channel for password resets; sharing it would let people take over each other's accounts.

## Examples

- Two warehouse scanners sharing one mailbox must each get a distinct address before verifying; plus-addressed variants of the same mailbox count as distinct addresses.
- Saving a profile email already used by another profile is refused as already in use.

## Exceptions

