---
id: FR-WSP-008
type: functional-requirement
title: "Role-based single sign-on enforcement"
status: draft
derived-from:
  - "UC-WSP-ENFORCE-SSO"
  - "UC-WSP-RESOLVE-SSO-NONCOMPLIANCE"
  - "BR-WSP-SSO-PER-ROLE"
  - "BR-WSP-SSO-COMPLIANCE"
verification:
  - scenario-ref: "SB-WSP-SSO-DOMAIN-MISMATCH"
  - scenario-ref: "SB-WSP-SSO-LOCAL-LOGIN"
  - scenario-ref: "SB-WSP-SSO-FREELANCER-UNAFFECTED"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let administrators enforce sign-in through a chosen identity provider for selected roles with allowed email domains, list non-compliant users, and refuse other sign-in methods for users of enforced roles.

## Rationale

Organizations need central control of internal staff authentication.
