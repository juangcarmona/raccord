---
id: BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
type: business-rule
title: Contact numbers are auto-assigned when empty
status: active
applies-to:
  - "UC-PRJ-ADD-CONTACT"
  - "UC-CON-MANAGE-CONTACT-DIRECTORY"
  - "UC-WSP-CONFIGURE-CONTACT-NUMBER-SERIES"
uses-terms:
  - "TERM-CONTACT-NUMBER"
  - "TERM-NUMBER-SERIES"
provenance:
  source: ".private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/360014106599.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A contact without a number receives the next available number from the configured contact-number series; the number is unique across automatically assigned and manually supplied values and is tied to its documents. Concurrent creation cannot assign a duplicate. Changing series settings never changes existing contact numbers.

## Rationale

Accounting needs stable references that remain unique and consistent with the workspace's numbering policy.

## Examples

A new contact left blank gets the next unused value from the configured series. A contact with a supplied number keeps that number if it is unique.

## Exceptions

A supplied duplicate contact number is rejected without changing existing records.
