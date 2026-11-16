---
id: FR-PRJ-034
type: functional-requirement
title: Extended contact import
status: draft
derived-from:
  - UC-PRJ-IMPORT-CONTACTS
  - BR-PRJ-CONTACT-IMPORT-MODES
  - BR-PRJ-CONTACT-PERSON-IMPORT-ROWS
  - BR-PRJ-IMPORT-FOLDER-PLACEMENT
  - CON-PRJ-IMPORT-FILE-FORMAT
verification:
  - scenario-ref: SB-PRJ-CONTACT-IMPORT-SKIP-EXISTING
  - scenario-ref: SB-PRJ-IMPORT-FAILED-ROWS-FILE
uses-terms:
  - TERM-CONTACT
provenance:
  source: .private/research/help-center/articles/360013553000.md
  confidence: high
  recovered-from: documentation
---

## Requirement

The extended profile UC-PRJ-IMPORT-CONTACTS describes importing contacts and contact persons in create, update or skip-existing mode, folder placement, import tagging and failed-row reporting. These extended capabilities remain outside the create-only profile FR-CON-003, whose supported fields, exclusions and audit privacy are defined independently.

## Rationale

Fast database set-up.
