---
id: BR-WSP-MASTER-CHANGES-NOT-RETROACTIVE
type: business-rule
title: "Master data changes do not update planned lines"
status: draft
applies-to:
  - "UC-WSP-BULK-EDIT"
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
  - "TERM-CONTACT"
provenance:
  source: ".private/research/help-center/articles/360013838520.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Changing values in the equipment or contact databases (for example a price or a customer discount) only affects what is planned afterwards; equipment already planned keeps its values until explicitly refreshed, and a project picks up a changed customer discount only when the contact is removed and added again with confirmation to overwrite.

## Rationale

Existing quotations and agreements must not change silently.

## Examples

- Raising an item's price does not change the price on yesterday's quotation.

## Exceptions

None.
