---
id: BR-WSP-EXTRA-ITEMS-OVERRIDE
type: business-rule
title: "Adding extra items in the warehouse overrides the booked-equipment restriction"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-BOOKED-EQUIPMENT-EDIT-RIGHT"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a role may add extra or other items in the warehouse, those users may always do so while packing, whatever the booked-equipment edit right says.

## Rationale

Replacing a broken item during packing must not be blocked.

## Examples

- A packer whose role forbids editing booked lines still swaps a broken unit for an alternative.

## Exceptions

None.
