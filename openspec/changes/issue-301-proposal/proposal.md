---
pdac-scope: cited
---

# Proposal

## Why

Workspace administrators need to create contacts from existing spreadsheets while reviewing errors before saving. The applied CHG-CONTACT-SPREADSHEET-IMPORT-301 intent, reconciled by CHG-CONTACTS-RECOVERY, defines this bounded create-only journey.
<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:9a964862aa671bf1c5f9648286ecf92c5ab60631ed7b30c02c3de8be38a3d587" -->

## What Changes

- Inspect and map CSV or single-sheet plain-text XLSX, preview the first 10 data rows and validate all rows before confirmation.
- Recheck the file and validation, create valid contacts independently, and report rejected row numbers and reasons.
- Enforce create permission and installation isolation; record minimal audit outcomes without uploaded values.
- Exclude updates, skip-existing, people, tags, folders, custom fields, undo, downloads of rejected rows, registry lookup and other record types. Do not add a product row limit.
<!-- pdac:cite id="FR-CON-003" digest="sha256:133563562f019c506fdf1f12725247c3798b2d0ef0e3d013a2557b7cbbc86e2c" -->
<!-- pdac:cite id="CON-CON-IMPORT-FILE-FORMAT" digest="sha256:f33969383814d4ba04d00eadd559a0dd470c67958d33cfc42ff6a8aded62f9fb" -->

## Capabilities

### New Capabilities

- `contacts-directory`: spreadsheet creation, validation, isolation and audit for contacts. No such capability exists in the current main spec inventory.

### Modified Capabilities

None.

## Impact

Contacts module and contracts, root-host authenticated endpoints, persistence, append-only audit, frontend routes and generated OpenAPI types are affected. No new production dependency or migration is planned. Existing `contacts-native-recovery` concerns recovery policy, not this runtime behavior. Impact neighbours checked include contact people, folder/tag and extended project import, commercial defaults and invoice/project selection; they are outside this profile.

This proposal is a candidate, not implementation approval. #443 corrected-head readiness and availability in the actual base, plus serial queue #437 activation, remain prerequisites to implementation.
