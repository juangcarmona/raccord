---
id: FR-WSP-014
type: functional-requirement
title: "Time-limited location contacts and file download levels"
status: draft
derived-from:
  - "BR-WSP-LOCATION-CONTACT-WINDOW"
  - "BR-WSP-FILE-DOWNLOAD-LEVELS"
  - "BR-WSP-CONFIDENTIAL-FIELDS"
verification:
  - scenario-ref: "SB-WSP-LOCATION-CONTACT-HIDDEN-AFTER"
  - scenario: "A role limited to non-confidential files cannot download a confidential file"
uses-terms:
  - "TERM-LOCATION-CONTACT-VISIBILITY"
  - "TERM-FILE-DOWNLOAD-LEVEL"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST restrict location contacts, file downloads and confidential custom fields according to the role's settings.

## Rationale

Protects sensitive information from crew who do not need it.
