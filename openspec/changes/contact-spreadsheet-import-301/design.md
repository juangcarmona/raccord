---
pdac-scope: cited
---

## Context

`ContactsDirectory` already validates and persists individual Contacts records through the Contacts module and unit of work. The API is same-origin and authenticated, the generated frontend client is checked for drift, and the compliance module exposes an append-only audit appender. See `proposal.md` and the delta spec for the accepted outcome.
<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:28c6c81be85f3cb0be1ea8156cc46c0b0ac712bed0dd3a5313ff693a6d501a75" -->
<!-- pdac:cite id="BR-CON-IMPORT-AUDIT" digest="sha256:ed358fb85c081e36b5dd42afa11d1dc3e3a268f9687d736d920a967c24de162b" -->

## Goals / Non-Goals

**Goals:** Parse a bounded spreadsheet without a production dependency, preview and validate it before confirmation, create valid rows independently, and write a minimal audit outcome.

**Non-Goals:** Persisting the upload or preview contents, asynchronous/background imports, duplicate matching, updates, import undo, and audit storage outside the existing append-only trail.

## Decisions

- Implement CSV and XLSX readers with platform libraries. CSV parsing handles quoted delimiters, escaped quotes, and newlines; XLSX reading accepts a single worksheet and plain text cells, rejects formulas, macros, embedded images and malformed ZIP/XML structures. A 10 MiB request limit and 64-column limit bound parser work.
- Keep preview stateless. The preview endpoint returns a SHA-256 digest, mapped field summary, the first 10 data rows, and validation outcomes for all rows. Confirmation resubmits the original file and mapping with that digest; the server hashes and parses it again, refuses a mismatch, and revalidates every row before writing. The app and audit trail never retain the upload or preview values.
- Use the existing Contacts use case for every row. Commit each valid contact separately through the Contacts unit of work so a rejected row cannot create a partial contact or roll back other valid rows. Unsupported format or mapping errors fail before the first write.
- Append a batch-start audit event before row writes and a completed summary after them, both keyed by a generated import id. Details contain only file type and accepted/rejected counts, never names, addresses, emails, tax IDs, or file contents. Use the current actor and server clock.
- Keep the UI in the existing Contacts feature boundary. It owns file selection, mapping, preview, confirmation, and rejected-row presentation; all domain validation and persistence remain server-side.
- Add no database migration: contacts use their existing persistence, and the current append-only audit table records import events.

## Risks / Trade-offs

- Re-parsing the file on confirmation costs extra CPU but avoids retaining raw contact data between requests and ensures the server rechecks exactly the previewed bytes.
- An interrupted batch may leave valid rows committed. The start audit event makes the attempt visible; each row is complete and independently committed, and the user can safely review the resulting directory before retrying.
- XLSX is a ZIP/XML format with attacker-controlled structure. Enforce the upload byte limit, reject unsupported workbook parts, avoid extracting to disk, and cap XML reader document/character sizes.

## Migration Plan

No data migration or rollback step is required. Deploy the API and UI together; reverting the application removes the import surface while leaving already created contacts and append-only audit facts intact.
