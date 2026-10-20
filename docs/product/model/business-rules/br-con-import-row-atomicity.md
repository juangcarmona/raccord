---
id: BR-CON-IMPORT-ROW-ATOMICITY
type: business-rule
title: "Contact import saves complete rows only"
status: active
applies-to:
  - "UC-CON-IMPORT-CONTACTS"
uses-terms:
  - "TERM-CONTACT"
---

## Rule

Each imported row either creates one complete contact or creates nothing. A rejected row does not prevent other valid rows in the same confirmed import from being created.

## Rationale

The import must not leave incomplete contacts while still allowing useful valid rows to be loaded.

## Examples

If a spreadsheet contains three valid rows and one row with an invalid tax identifier, the three contacts are created and the invalid row is reported without a partial contact.

## Exceptions

None.
