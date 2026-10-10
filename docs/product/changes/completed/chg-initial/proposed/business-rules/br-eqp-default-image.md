---
id: BR-EQP-DEFAULT-IMAGE
type: business-rule
title: "First non-confidential image is the default"
status: draft
applies-to:
  - "UC-EQP-ADD-ITEM-IMAGE"
uses-terms:
  - "TERM-EQUIPMENT-IMAGE"
provenance:
  source: ".private/research/help-center/articles/12687358328338.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The first image uploaded to an item that is not marked confidential becomes the item's default image.

## Rationale

Gives every item a representative picture automatically.

## Examples

- Uploading a confidential receipt scan, then a product photo, makes the photo the default image.

## Exceptions

None.
