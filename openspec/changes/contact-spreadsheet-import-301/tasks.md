---
pdac-scope: cited
---

## Parser and domain validation

- [x] Add failing tests for CSV quoting, optional headers, field mapping, blank/duplicate headers, malformed rows, max columns, and supported contact fields.
- [x] Add failing tests for XLSX one-sheet/plain-text handling and rejection of multiple sheets, formulas, macros, embedded images, malformed archives, and oversized files.
- [x] Implement bounded CSV/XLSX parsing with the .NET platform libraries and row-level validation results; keep uploaded bytes and values transient.
- [x] Add domain/use-case tests proving each accepted row is validated using the existing Contacts creation rules.
<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:28c6c81be85f3cb0be1ea8156cc46c0b0ac712bed0dd3a5313ff693a6d501a75" -->
<!-- pdac:cite id="CON-CON-IMPORT-FILE-FORMAT" digest="sha256:42f4cbb77ecbc40f73e0ab222bf28dd104d22e7ad6b2f47d9a497cfae0773a49" -->

## Import application and API

- [x] Add failing API tests for preview and confirmation, upload-size enforcement, digest mismatch, authorization, all-invalid input, mixed valid/invalid rows, and per-row persistence atomicity.
- [x] Implement preview and confirm contracts with bounded multipart requests; recompute and compare the digest on confirmation and revalidate rows before writes.
- [x] Persist each valid contact independently and return accepted count plus every rejected row number and reason; malformed files or mappings must fail before writes.
- [x] Add failing audit tests for actor, server timestamp, import id, file type, counts, and the absence of uploaded contact values; then append start and completion audit facts.
- [x] Regenerate the OpenAPI schema and typed frontend client; verify contract drift is empty.
<!-- pdac:cite id="BR-CON-IMPORT-ROW-ATOMICITY" digest="sha256:b762dc5819168c4d0bcc6fe2fd7b4826b8a6e0efa5f0afec7b6f7abbd018926b" -->
<!-- pdac:cite id="BR-CON-IMPORT-AUDIT" digest="sha256:ed358fb85c081e36b5dd42afa11d1dc3e3a268f9687d736d920a967c24de162b" -->

## Contacts UI and end-to-end evidence

- [x] Add frontend tests for file selection, mapping, preview, validation summaries, confirmation, rejected rows, and API errors.
- [x] Add the authenticated Contacts import flow with an accessible upload, field mapping, preview table, confirmation action, and rejection report.
- [x] Add a synthetic CSV fixture and a Playwright journey that uploads it, previews and confirms the valid row, shows the rejected row, and reads the audit outcome without exposing raw values there.
- [x] Add the journey to `tests/Raccord.EndToEndTests/per-pr-selection.txt` with `Area=contacts` and cite `SB-CON-IMPORT-VALID-AND-INVALID-ROWS` in its docstring.
- [x] Update the process ledger with any durable lesson and verify its existing checks; run formatters, focused tests, contract generation, selected Aspire E2E, and the full CI-equivalent checks on the final PR head.
<!-- pdac:cite id="SB-CON-IMPORT-VALID-AND-INVALID-ROWS" digest="sha256:0af65b9f6849d2276fdcebb26929b5378c86ddf31c4f84452b5ae5c57a813d9a" -->
<!-- pdac:cite id="FR-CON-003" digest="sha256:4d08fb6282312db4988c5987bc65d9677fe56edf51b8e63547711008185ffd00" -->
