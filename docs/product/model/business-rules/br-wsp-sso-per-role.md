---
id: BR-WSP-SSO-PER-ROLE
type: business-rule
title: "Single sign-on is enforced per role, never per person"
status: draft
applies-to:
  - "UC-WSP-ENFORCE-SSO"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Single sign-on enforcement is switched on for selected user roles; every user holding such a role must sign in through the chosen identity provider, and users of other roles (for example external freelancers) keep their usual sign-in.

## Rationale

Internal staff can be bound to the company directory without excluding external crew who have no identity there.

## Examples

- Enforcing on the office role but not the freelancer role lets freelancers keep password sign-in.

## Exceptions

None.
