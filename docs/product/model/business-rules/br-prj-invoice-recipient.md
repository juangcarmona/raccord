---
id: BR-PRJ-INVOICE-RECIPIENT
type: business-rule
title: "Invoices go to the administrative contact person"
status: draft
applies-to:
  - "UC-PRJ-ADD-CONTACT-PERSON"
uses-terms:
  - "TERM-ADMINISTRATIVE-CONTACT-PERSON"
  - "TERM-DEFAULT-CONTACT-PERSON"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When an invoice is sent, the administrative contact person is selected as recipient; without one, the default contact person is used.

## Rationale

Finance departments receive invoices.

## Examples

See SB-PRJ-INVOICE-TO-ADMINISTRATIVE-CONTACT.

## Exceptions

None.
