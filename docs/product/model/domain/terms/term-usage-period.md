---
id: TERM-USAGE-PERIOD
type: domain-term
title: "Usage period"
status: draft
defined-in: BC-EQUIPMENT
synonyms:
  - "charge period"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-PLANNED-EQUIPMENT"
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/360017647460.md, .private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The interval for which planned equipment is charged to the customer. It determines the price factor and the line total of each planned line. Rental duration counts each calendar day touched by the usage period. Like the planning period it is a reference to a time-schedule entry and defaults from its equipment group or the project. When no dates are needed (for example in pure sales projects) a duration can be entered instead, and the factor is computed from that duration.

## Distinguish From

Not the planning period: extending the planning period (for instance because of a delay) does not change the usage period, so it does not change what is charged.

## Usage

Used by pricing; ignored by availability.
