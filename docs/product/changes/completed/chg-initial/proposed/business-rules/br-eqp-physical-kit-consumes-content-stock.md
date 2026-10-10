---
id: BR-EQP-PHYSICAL-KIT-CONSUMES-CONTENT-STOCK
type: business-rule
title: "Physical kit content leaves free stock"
status: draft
applies-to:
  - "UC-EQP-DEFINE-PHYSICAL-KIT"
uses-terms:
  - "TERM-PHYSICAL-KIT"
  - "TERM-CONTENT-STOCK-BINDING"
  - "TERM-STOCK-QUANTITY"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/19895983469330.md"
  confidence: high
  recovered-from: documentation
---

## Rule

For each default content line of a physical kit that is reserved from stock, the free stock of the content item is reduced by the kit's stock quantity multiplied by the line quantity, and that content cannot be planned individually.

## Rationale

The content is physically inside the cases.

## Examples

- Kit quantity 5 with 3 of item B per kit lowers item B's free stock by 15.

## Exceptions

Lines marked available outside the kit, and sale items as content, are not deducted.
