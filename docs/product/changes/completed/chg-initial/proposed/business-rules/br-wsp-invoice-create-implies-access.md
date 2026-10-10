---
id: BR-WSP-INVOICE-CREATE-IMPLIES-ACCESS
type: business-rule
title: "Creating invoices implies access to invoices"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-ROLE-PERMISSION"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A role allowed to create invoices from projects automatically has access to the invoices area.

## Rationale

Users must be able to find what they created.

## Examples

- Granting invoice creation enables the invoice list for the role.

## Exceptions

None.
