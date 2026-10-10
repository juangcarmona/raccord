---
id: FR-WSP-009
type: functional-requirement
title: "Breached password warning"
status: draft
derived-from:
  - "BR-WSP-BREACHED-PASSWORD-CHECK"
verification:
  - scenario-ref: "SB-WSP-BREACHED-PASSWORD"
uses-terms:
  - "TERM-BREACHED-PASSWORD-WARNING"
provenance:
  source: ".private/research/help-center/articles/31560381502226.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST warn at registration and sign-in when a password appears in public breach lists, without disclosing the password.

## Rationale

Reduces account takeover through reused passwords.
