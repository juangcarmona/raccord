---
id: SB-WSP-PRICE-CHANGE-NOT-RETROACTIVE
type: structured-behaviour
title: "Price change leaves planned equipment untouched"
status: draft
illustrates:
  - "BR-WSP-MASTER-CHANGES-NOT-RETROACTIVE"
given:
  - "An item is planned on a project with a sent quotation"
when: "The equipment manager raises the item's price in the catalog"
then:
  - "The existing quotation keeps the old price"
  - "Equipment planned afterwards uses the new price"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013838520.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Catalog changes are not retroactive.

## Boundaries

None.
