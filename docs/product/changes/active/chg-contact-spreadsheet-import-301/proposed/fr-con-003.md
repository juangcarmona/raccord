---
id: FR-CON-003
type: functional-requirement
title: "Import contacts with row validation and audit"
status: active
derived-from:
  - "UC-CON-IMPORT-CONTACTS"
  - "BR-CON-IMPORT-ROW-ATOMICITY"
  - "BR-CON-IMPORT-AUDIT"
verification:
  - scenario-ref: "SB-CON-IMPORT-VALID-AND-INVALID-ROWS"
uses-terms:
  - "TERM-CONTACT"
---

## Requirement

The product MUST let an authorized user upload a supported spreadsheet, map its columns to supported contact fields, preview the mapped values and validation results, and confirm creation of valid contact rows. A row MUST be persisted in full or not persisted. Valid rows MUST be importable when other rows are rejected. The product MUST report each rejected row and its reason and MUST audit the actor, time and outcome summary without retaining the uploaded file or raw contact values.

## Rationale

Users can set up the directory in bulk while preventing incomplete records and retaining a minimal audit trail.
