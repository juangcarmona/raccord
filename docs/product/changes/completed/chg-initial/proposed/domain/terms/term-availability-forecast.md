---
id: TERM-AVAILABILITY-FORECAST
type: domain-term
title: "Availability forecast"
status: draft
defined-in: BC-EQUIPMENT
synonyms: []
uses-terms:
  - "TERM-EQUIPMENT-AVAILABILITY"
  - "TERM-RESERVABLE-STOCK"
  - "TERM-SOFT-HOLD"
provenance:
  source: ".private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360013158280.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The reading of availability that subtracts every planned quantity whose project status consumes availability (inquiry, draft, pending, confirmed and later), whether or not it is reserved. It answers: how many would be left if all these projects went ahead.

## Distinguish From

Differs from reservable stock, which subtracts only reserved quantities.

## Usage

Top line of the availability timeline and the number given by the quick availability check.
