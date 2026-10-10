---
id: FR-WSP-022
type: functional-requirement
title: "Bulk edit"
status: draft
derived-from:
  - "UC-WSP-BULK-EDIT"
  - "BR-WSP-BULK-EDIT-KIND-FIELDS"
  - "BR-WSP-MASTER-CHANGES-NOT-RETROACTIVE"
verification:
  - scenario-ref: "SB-WSP-VIRTUAL-KIT-NO-QUANTITY"
  - scenario-ref: "SB-WSP-PRICE-CHANGE-NOT-RETROACTIVE"
uses-terms:
  - "TERM-BULK-EDIT"
provenance:
  source: ".private/research/help-center/articles/360013838520.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users set chosen fields to one value on several selected records, offering only fields applicable to the selection and leaving planned lines unchanged.

## Rationale

Saves time on repetitive edits.
