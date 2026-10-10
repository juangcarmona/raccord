---
id: BR-WSP-BULK-EDIT-KIND-FIELDS
type: business-rule
title: "Bulk edit only changes fields valid for the selected kinds"
status: draft
applies-to:
  - "UC-WSP-BULK-EDIT"
uses-terms:
  - "TERM-BULK-EDIT"
  - "TERM-EQUIPMENT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013838520.md"
  confidence: high
  recovered-from: documentation
---

## Rule

In a bulk edit only fields that apply to the kind of the selected records can be changed; for equipment some fields apply only to single items and others only to virtual kits, whose quantity is derived from their content and cannot be set.

## Rationale

Avoids writing meaningless values.

## Examples

- Selecting virtual kits does not allow changing a stock quantity.

## Exceptions

None.
