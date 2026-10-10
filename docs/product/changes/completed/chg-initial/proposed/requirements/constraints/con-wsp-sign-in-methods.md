---
id: CON-WSP-SIGN-IN-METHODS
type: constraint
title: "Supported sign-in methods"
status: draft
applies-to:
  - "UC-WSP-SIGN-IN"
  - "UC-WSP-ENFORCE-SSO"
uses-terms:
  - "TERM-EXTERNAL-SIGN-IN"
  - "TERM-SSO-ENFORCEMENT"
provenance:
  source: ".private/research/help-center/articles/360018767240.md, .private/research/help-center/articles/360014204180.md, .private/research/help-center/articles/17043308502290.md"
  confidence: medium
  recovered-from: documentation
---

## Constraint

A personal profile signs in with a verified email address and password or with a Microsoft or Google account; single sign-on enforcement can only use one of these supported providers, and multi-factor authentication is not provided by the product itself.

## Rationale

The product relies on established providers for federated identity.

## Consequences

- Organizations requiring additional factors must enforce them at their identity provider.
