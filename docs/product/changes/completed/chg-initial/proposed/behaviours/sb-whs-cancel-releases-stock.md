---
id: SB-WHS-CANCEL-RELEASES-STOCK
type: structured-behaviour
title: "Cancelling a project frees its stock"
status: draft
illustrates:
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
given:
  - "A confirmed project reserves 3 of 3 owned mixers"
  - "A pending project plans 2 mixers in the same period and shows a shortage of 2"
when: "The confirmed project is cancelled"
then:
  - "The cancelled project consumes no availability and holds no reservation"
  - "The pending project's shortage changes to unreserved own stock"
uses-terms:
  - "TERM-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360013836939.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Cancelled plans have no effect on stock.

## Boundaries

Does not assert that the pending project reserves automatically.
