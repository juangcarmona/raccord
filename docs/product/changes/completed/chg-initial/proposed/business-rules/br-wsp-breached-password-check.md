---
id: BR-WSP-BREACHED-PASSWORD-CHECK
type: business-rule
title: "Breached passwords are flagged without disclosing them"
status: draft
applies-to:
  - "UC-WSP-SIGN-IN"
  - "UC-WSP-CREATE-WORKSPACE"
uses-terms:
  - "TERM-BREACHED-PASSWORD-WARNING"
provenance:
  source: ".private/research/help-center/articles/31560381502226.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

At registration and at sign-in the product checks the password against public breach lists using a method that never sends the password or anything that reveals it outside the product, and warns the user when it is found; the warning recommends changing to a unique password.

## Rationale

Reused leaked passwords are the main cause of account takeover.

## Examples

- Signing in with a common leaked password shows the warning; the user may change the password right away.

## Exceptions

- The warning does not mean the account was breached; it does not by itself block sign-in.
