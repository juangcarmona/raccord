---
id: BR-WSP-EMAIL-CHANGE-PROPAGATES
type: business-rule
title: "A verified email change updates the connected crew member"
status: draft
applies-to:
  - "UC-WSP-CHANGE-PROFILE-EMAIL"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/15867463701394.md"
  confidence: high
  recovered-from: documentation
---

## Rule

After the person confirms a changed profile email through the verification link, the new address is used for sign-in from then on and replaces the email on the connected crew-member record.

## Rationale

Keeps the company's contact address in step with the person's identity.

## Examples

- Changing from a personal to a work address updates the crew member's email once confirmed.

## Exceptions

- Until the link is confirmed the old address remains in force.
