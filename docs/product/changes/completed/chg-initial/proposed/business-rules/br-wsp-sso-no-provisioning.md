---
id: BR-WSP-SSO-NO-PROVISIONING
type: business-rule
title: "Identity provider does not provision users"
status: draft
applies-to:
  - "UC-WSP-ENFORCE-SSO"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Enforcing single sign-on does not create, update or remove users: users are still added and removed in the workspace by hand, and multi-factor authentication is delegated entirely to the provider.

## Rationale

Keeps the product the place where workspace membership and roles are decided.

## Examples

- Removing an employee from the company directory does not delete their crew member; an administrator still disables their login.

## Exceptions

None.
