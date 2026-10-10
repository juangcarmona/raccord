---
id: SB-EQP-PHYSICAL-KIT-DEDUCTS-CONTENT
type: structured-behaviour
title: "Physical kit quantity deducts content stock"
status: draft
illustrates:
  - "BR-EQP-PHYSICAL-KIT-CONSUMES-CONTENT-STOCK"
given:
  - "Each kit holds 1 item A, 3 item B and 4 item C, all reserved from stock"
when: "The physical kit is saved with stock quantity 5"
then:
  - "Free stock of item A drops by 5"
  - "Free stock of item B drops by 15"
  - "Free stock of item C drops by 20"
uses-terms:
  - "TERM-PHYSICAL-KIT"
  - "TERM-CONTENT-STOCK-BINDING"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the multiplication of kit quantity by content quantity.

## Boundaries

None.
