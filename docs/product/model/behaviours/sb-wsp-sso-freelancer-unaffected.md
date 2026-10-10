---
id: SB-WSP-SSO-FREELANCER-UNAFFECTED
type: structured-behaviour
title: "Roles without enforcement keep their sign-in"
status: draft
illustrates:
  - "BR-WSP-SSO-PER-ROLE"
given:
  - "Single sign-on is enforced for the office role only"
when: "A user with the Crew role signs in with email and password"
then:
  - "Sign-in succeeds without the identity provider"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Intent

External crew are not forced into the company directory.

## Boundaries

None.
