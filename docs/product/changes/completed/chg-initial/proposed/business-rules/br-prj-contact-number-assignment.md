---
id: BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
type: business-rule
title: "Contact numbers are auto-assigned when empty"
status: draft
applies-to:
  - "UC-PRJ-ADD-CONTACT"
uses-terms:
  - "TERM-CONTACT-NUMBER"
  - "TERM-NUMBER-SERIES"
provenance:
  source: ".private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/360014106599.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A contact without a number receives the next number from the contact number series; the number is unique and tied to its documents.

## Rationale

Accounting needs stable references.

## Examples

A new contact left blank gets 10235.

## Exceptions

None.
