---
id: FR-PRJ-101
type: functional-requirement
title: "Change several contacts at once"
status: draft
derived-from:
  - "UC-PRJ-BULK-EDIT-CONTACTS"
verification:
  - id: "BULK-EDIT-CONTACTS"
    scenario: "Given five selected contacts, when the user chooses a field and a new value, all five contacts show the value; a user without contact editing rights cannot do it."
uses-terms:
  - "TERM-CONTACT"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a user who may edit contacts select several contacts and the fields to change and apply the given values to all selected contacts, and MUST support export, edit and re-import as an alternative.

## Rationale

A common value changing should not require visiting every contact.
