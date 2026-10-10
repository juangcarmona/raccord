---
id: TERM-SSO-ENFORCEMENT
type: domain-term
title: "Single sign-on enforcement"
status: draft
defined-in: "BC-WORKSPACE"
synonyms:
  - "enforced SSO"
uses-terms:
  - "TERM-USER-ROLE"
  - "TERM-LOGIN-DOMAIN-RESTRICTION"
  - "TERM-PERSONAL-PROFILE"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md, .private/research/help-center/articles/36290993235730.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A company security policy that forces users holding selected user roles to sign in only through a chosen identity provider. Attributes: provider, list of allowed email domains, enforced roles. Users in enforced roles must have a verified email address in an allowed domain; the provider's own policies (additional factors, password rotation) then apply. It does not provision or remove users.

## Distinguish From

Not external sign-in, which any profile may choose voluntarily. Not role permissions, which govern what a user may do after signing in.

## Usage

Configured by users with access to configuration; checked at sign-in and when assigning roles.
