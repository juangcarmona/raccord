---
id: BR-FIN-QUOTE-VALIDITY-DEFAULT
type: business-rule
title: "Quotes expire after a configurable default period"
status: draft
applies-to:
  - "UC-FIN-GENERATE-QUOTE"
  - "UC-FIN-CONFIGURE-FINANCIAL-SETTINGS"
uses-terms:
  - "TERM-QUOTE-VALIDITY"
  - "TERM-EXPIRED-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Each generated quote or contract expires after the default expiration period (14 days unless the company configures another number of days); the expiration date can be changed per document and the change overrides the default; the date is printed on the document.

## Rationale

Prices and availability cannot be promised indefinitely.

## Examples

See SB-FIN-QUOTE-EXPIRES-AFTER-14-DAYS.

## Exceptions

None.
