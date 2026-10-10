---
id: FR-INT-102
type: functional-requirement
title: "Leave planned equipment behind while packing"
status: draft
derived-from:
  - "UC-INT-LEAVE-EQUIPMENT-BEHIND"
  - "BR-INT-LEAVE-BEHIND-REMOVES-LINE"
verification:
  - scenario-ref: "SB-INT-LEAVE-BEHIND"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/37629880908050.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST, for a technician holding the leave-behind permission, allow marking a planned item as left behind while booking, MUST remove the item from the project and MUST derive the packing moment's status from the remaining items. The product MUST NOT offer the action without the permission.

## Rationale

Damaged or over-ordered equipment should not block the packing moment. The product owner confirmed that leaving equipment behind removes it from this project's plan; the remaining planned items determine the packing moment's status.
