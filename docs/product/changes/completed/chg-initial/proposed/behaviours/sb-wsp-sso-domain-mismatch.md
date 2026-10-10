---
id: SB-WSP-SSO-DOMAIN-MISMATCH
type: structured-behaviour
title: "User outside allowed domains is non-compliant"
status: draft
illustrates:
  - "BR-WSP-SSO-COMPLIANCE"
given:
  - "The allowed domain is the company domain"
  - "A user of the role has an address at another domain"
when: "The administrator enforces single sign-on for that role"
then:
  - "The product reports that users in the role do not meet domain restrictions"
  - "The user is listed as non-compliant"
  - "The user cannot sign in until resolved"
uses-terms:
  - "TERM-LOGIN-DOMAIN-RESTRICTION"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows the domain check.

## Boundaries

None.
