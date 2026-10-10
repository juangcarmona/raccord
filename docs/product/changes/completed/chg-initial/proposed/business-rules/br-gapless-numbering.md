---
id: BR-GAPLESS-NUMBERING
type: business-rule
title: "Numbers are correlative and gapless per series and year"
status: draft
applies-to:
  - "UC-ISSUE-INVOICE"
uses-terms:
  - "TERM-INVOICE-NUMBER"
  - "TERM-INVOICE-SERIES"
---

## Rule

An invoice number is allocated only when an invoice is issued, as the next correlative number of its series in the calendar year of its issue date; a refused or failed issue consumes no number and no number is ever reused.

## Rationale

Correlative numbering per series is a legal requirement and a basic audit expectation.

## Examples

See SB-FIRST-INVOICE-NUMBER and SB-CONCURRENT-ISSUING-GAPLESS.

## Exceptions

None.
