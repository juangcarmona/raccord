---
id: FR-WSP-003
type: functional-requirement
title: "Unique login addresses"
status: draft
derived-from:
  - "BR-WSP-ONE-PROFILE-PER-EMAIL"
  - "BR-WSP-VERIFY-UNIQUE-IN-WORKSPACE"
verification:
  - scenario-ref: "SB-WSP-DUPLICATE-EMAIL-IN-WORKSPACE"
  - scenario-ref: "SB-WSP-PLUS-ADDRESSES-DISTINCT"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
provenance:
  source: ".private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36290993235730.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST refuse a verified email address already used by another profile or by another user, and flag duplicates to administrators.

## Rationale

Shared addresses allow account takeover.
