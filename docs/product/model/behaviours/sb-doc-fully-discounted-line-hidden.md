---
id: "SB-DOC-FULLY-DISCOUNTED-LINE-HIDDEN"
type: structured-behaviour
title: "Fully discounted lines disappear with zero-price hiding"
status: draft
illustrates:
  - "BR-DOC-HIDE-ZERO-PRICED"
given:
  - "Quotations hide zero-priced lines"
  - "An accessory is priced at 0"
  - "A microphone has a 100% discount"
when: "A quotation is generated"
then:
  - "The zero-priced accessory is not listed"
  - "The 100%-discounted microphone is not listed either"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360017647860.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Users must know hiding zeros also hides gifts.

## Boundaries

None.
