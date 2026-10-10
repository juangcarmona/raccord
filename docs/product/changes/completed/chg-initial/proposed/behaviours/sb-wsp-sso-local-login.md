---
id: SB-WSP-SSO-LOCAL-LOGIN
type: structured-behaviour
title: "User with an unverified address cannot hold an enforced role"
status: draft
illustrates:
  - "BR-WSP-SSO-COMPLIANCE"
given:
  - "A user has not yet verified an email address"
when: "Single sign-on is enforced for that user's role"
then:
  - "The user is reported as non-compliant because there is no verified address in an allowed domain"
uses-terms:
  - "TERM-LOGIN-STATUS"
  - "TERM-SSO-ENFORCEMENT"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Enforcement needs a verified identity.

## Boundaries

None.
