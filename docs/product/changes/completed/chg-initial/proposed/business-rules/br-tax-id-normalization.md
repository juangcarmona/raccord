---
id: BR-TAX-ID-NORMALIZATION
type: business-rule
title: "Tax identifiers are normalized"
status: draft
applies-to:
  - "UC-REGISTER-CUSTOMER"
  - "UC-DEFINE-ISSUER-PROFILE"
uses-terms:
  - "TERM-TAX-ID"
---

## Rule

Tax identifiers are stored in uppercase with spaces, dots and hyphens removed; an empty value means unknown.

## Rationale

The same identifier typed differently must be the same identifier.

## Examples

" a-00000000 " is stored as A00000000.

## Exceptions

None.
