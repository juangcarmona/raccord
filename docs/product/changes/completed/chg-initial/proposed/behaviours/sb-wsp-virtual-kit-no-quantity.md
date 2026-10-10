---
id: SB-WSP-VIRTUAL-KIT-NO-QUANTITY
type: structured-behaviour
title: "Bulk edit of virtual kits offers no quantity"
status: draft
illustrates:
  - "BR-WSP-BULK-EDIT-KIND-FIELDS"
when: "The user starts a bulk edit on a selection of virtual kits"
then:
  - "Stock quantity is not among the editable fields"
uses-terms:
  - "TERM-BULK-EDIT"
provenance:
  source: ".private/research/help-center/articles/360013838520.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Derived values are never set by bulk edit.

## Boundaries

None.
