---
id: FR-HOSTING-002
type: functional-requirement
title: "Back up and restore installations"
status: draft
derived-from:
  - "UC-BACK-UP-RACCORD"
  - "UC-RESTORE-RACCORD"
  - "CON-SELF-HOSTABLE"
verification:
  - scenario-ref: "SB-RESTORE-PRESERVES-FINGERPRINTS"
uses-terms: []
---

## Requirement

The product MUST provide backup and restore of all records and stored documents, with checksums, such that restored issued invoices keep their fingerprints and protections.

## Rationale

Invoices are legal records.
