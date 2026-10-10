---
id: "BR-TRB-RESET-NEEDS-UNIQUE-EMAIL"
type: business-rule
title: "Password reset requires a unique e-mail address"
status: draft
applies-to:
  - "UC-WSP-MANAGE-PASSWORD"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014048499.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A password reset by e-mail can only be completed when no other user shares the e-mail address entered; when two users have the same address, the address of one must first be changed in their login information.

## Rationale

The reset link must identify exactly one login.

## Examples

See SB-TRB-RESET-DUPLICATE-EMAIL.

## Exceptions

None.
