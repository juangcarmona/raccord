---
id: BR-WSP-SSO-COMPLIANCE
type: business-rule
title: "Users of enforced roles must be compliant to sign in"
status: draft
applies-to:
  - "UC-WSP-ENFORCE-SSO"
  - "UC-WSP-RESOLVE-SSO-NONCOMPLIANCE"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
  - "TERM-LOGIN-DOMAIN-RESTRICTION"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A user in an SSO-enforced role is compliant only when the domain of their verified email address is among the allowed domains; non-compliant users cannot sign in until they get another role or fix their email.

## Rationale

Enforcement is only meaningful for identities the provider controls.

## Examples

- A user with an address at a free consumer mail domain in a role restricted to the company domain is listed as non-compliant.
- A user whose email address is not yet verified cannot keep an enforced role.

## Exceptions

None.
