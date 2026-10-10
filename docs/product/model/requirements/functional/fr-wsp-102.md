---
id: FR-WSP-102
type: functional-requirement
title: "Set, change or reset a profile password"
status: draft
derived-from:
  - "UC-WSP-MANAGE-PASSWORD"
  - "BR-WSP-BREACHED-PASSWORD-CHECK"
verification:
  - scenario-ref: "SB-TRB-RESET-DUPLICATE-EMAIL"
  - id: "BREACHED-PASSWORD-WARNING"
    scenario: "Given a new password that appears in breach lists, when the person saves it, the product warns about it."
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-EXTERNAL-SIGN-IN"
  - "TERM-BREACHED-PASSWORD-WARNING"
provenance:
  source: ".private/research/help-center/articles/15867463701394.md, .private/research/help-center/articles/360014997360.md, .private/research/help-center/articles/4402790420882.md, .private/research/help-center/articles/31560381502226.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a person with a profile set or change the password in the profile settings, including a person who so far has only an external sign-in, and MUST accept the new password for subsequent sign-ins. The product MUST let a person who forgot the password request a reset link by e-mail from the sign-in screen, and MUST warn when the chosen password appears in breach lists.

## Rationale

People must stay in control of access to their own profile.
