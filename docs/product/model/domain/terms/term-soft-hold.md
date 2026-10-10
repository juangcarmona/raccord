---
id: TERM-SOFT-HOLD
type: domain-term
title: "Soft hold"
status: draft
defined-in: BC-EQUIPMENT
synonyms:
  - "unreserved plan"
  - "tentative hold"
uses-terms:
  - "TERM-AVAILABILITY-FORECAST"
  - "TERM-RESERVATION-STATE"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360013730520.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

Planned demand that lowers the availability forecast without securing stock. It arises from planned lines in inquiry, draft and pending projects (and from confirmed lines whose quantity could not be reserved). A soft hold never protects the project against other projects reserving the same units first.

## Distinguish From

Not a reservation; not a cancelled plan (which has no effect at all).

## Usage

Warns other planners early that stock is wanted while leaving priority open.
