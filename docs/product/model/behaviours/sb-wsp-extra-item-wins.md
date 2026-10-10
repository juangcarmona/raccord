---
id: SB-WSP-EXTRA-ITEM-WINS
type: structured-behaviour
title: "Extra-item right overrides booked-line protection"
status: draft
illustrates:
  - "BR-WSP-EXTRA-ITEMS-OVERRIDE"
given:
  - "A role forbids editing booked equipment"
  - "The same role may add extra or other items in the warehouse"
when: "The packer replaces a broken booked unit with another item during packing"
then:
  - "The replacement is accepted"
uses-terms:
  - "TERM-BOOKED-EQUIPMENT-EDIT-RIGHT"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Documents the precedence between two warehouse permissions.

## Boundaries

None.
