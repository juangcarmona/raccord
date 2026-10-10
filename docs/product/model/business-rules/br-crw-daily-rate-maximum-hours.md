---
id: BR-CRW-DAILY-RATE-MAXIMUM-HOURS
type: business-rule
title: "Hours beyond the daily maximum are charged hourly on top of the daily rate"
status: draft
applies-to:
  - "UC-CRW-LINK-RATES-TO-CREW-MEMBER"
uses-terms:
  - "TERM-CREW-RATE"
provenance:
  source: ".private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

For a rate with an hourly rate, a daily rate and a maximum number of daily hours, hours worked in a day beyond that maximum are charged at the hourly rate in addition to the daily rate.

## Rationale

Simple day-rate agreements with overtime.

## Examples

Daily 400, hourly 50, maximum 10: an 11-hour day costs 450.

## Exceptions

None.
