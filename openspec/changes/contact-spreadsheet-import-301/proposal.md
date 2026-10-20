---
pdac-scope: cited
---

## Why

Workspace administrators need a safe way to create many Contacts records from a spreadsheet, review data before saving, and understand which rows were rejected. The accepted product change is `CHG-CONTACT-SPREADSHEET-IMPORT-301`.
<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:28c6c81be85f3cb0be1ea8156cc46c0b0ac712bed0dd3a5313ff693a6d501a75" -->
<!-- pdac:cite id="FR-CON-003" digest="sha256:4d08fb6282312db4988c5987bc65d9677fe56edf51b8e63547711008185ffd00" -->

## What Changes

- Add a Contacts import screen for CSV and XLSX files with a 10 MiB upload limit, one worksheet, at most 64 columns, optional headers, plain text cells, and a preview of the first 10 data rows.
- Let the administrator map spreadsheet columns to accepted contact fields, validate all rows, and import each valid row atomically while showing rejected row numbers and reasons.
- Record import actor, time, and accepted/rejected counts without retaining the uploaded file or raw contact values.
- Keep imports limited to new contacts; matching, updates, persons, tags, folders, undo, and downloadable correction files stay outside this change.
<!-- pdac:cite id="CON-CON-IMPORT-FILE-FORMAT" digest="sha256:42f4cbb77ecbc40f73e0ab222bf28dd104d22e7ad6b2f47d9a497cfae0773a49" -->
<!-- pdac:cite id="BR-CON-IMPORT-ROW-ATOMICITY" digest="sha256:b762dc5819168c4d0bcc6fe2fd7b4826b8a6e0efa5f0afec7b6f7abbd018926b" -->
<!-- pdac:cite id="BR-CON-IMPORT-AUDIT" digest="sha256:ed358fb85c081e36b5dd42afa11d1dc3e3a268f9687d736d920a967c24de162b" -->

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `contacts-directory`: add safe spreadsheet preview, mapping, validation, row-atomic import, rejected-row feedback, and minimal audit outcome.

## Impact

Affected code includes the Contacts module, its public contracts, persistence and migration, authenticated Contacts routes and UI, generated OpenAPI types, and Aspire Playwright journeys. The proposal's structural impact review also checked the Contacts bounded context, contact type/person terms, customer term, commercial-default term, and linked invoicing/commercial terms; those neighboring behaviors are outside this change. No production dependency is required: CSV and XLSX parsing will use platform libraries.
<!-- pdac:cite id="SB-CON-IMPORT-VALID-AND-INVALID-ROWS" digest="sha256:0af65b9f6849d2276fdcebb26929b5378c86ddf31c4f84452b5ae5c57a813d9a" -->
