---
id: SB-CON-IMPORT-RECHECKS-PREVIEW
type: structured-behaviour
title: Recheck the previewed import before writing
status: active
illustrates:
  - UC-CON-IMPORT-CONTACTS
  - CON-CON-IMPORT-FILE-FORMAT
given:
  - An administrator has reviewed a mapped import preview
when: The administrator confirms the import
then:
  - The product verifies the confirmed file digest against the preview and validates rows again before writes
  - A mismatching file or invalid mapping creates no contacts
  - Accepted files follow row atomicity and retain no uploaded values in audit data
---

## Intent

Recheck the previewed import before writing.

## Boundaries

This is create-only import; no tags, folder placement, contact persons, updates, skip-existing or undo are included.
