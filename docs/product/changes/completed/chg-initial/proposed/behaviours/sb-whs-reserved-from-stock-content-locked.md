---
id: SB-WHS-RESERVED-FROM-STOCK-CONTENT-LOCKED
type: structured-behaviour
title: "Reserved-from-stock content cannot change"
status: draft
illustrates:
  - "BR-WHS-KIT-CONTENT-EDITABILITY"
  - "UC-WHS-ADJUST-KIT-CONTENT"
given:
  - "A planned virtual combination allows editing"
  - "One of its content articles is reserved from stock"
when: "The coordinator tries to lower that content's quantity"
then:
  - "The change is refused"
  - "Other content can still be edited"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/19895194067730.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Editability has an exception.

## Boundaries

None.
