---
id: QR-WSP-VERIFICATION-LINK-LIFETIME
type: quality-requirement
title: "Verification links valid 24 hours"
status: draft
quality-attribute: "security"
applies-to:
  - "UC-WSP-CHANGE-PROFILE-EMAIL"
verification:
  - scenario-ref: "SB-WSP-LINK-EXPIRED"
uses-terms:
  - "TERM-VERIFICATION-LINK"
provenance:
  source: ".private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36291020702098.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

Email verification links MUST be accepted for 24 hours after sending and refused afterwards.

## Measurement

Open links at 23h59 (accepted) and 24h01 (refused).
