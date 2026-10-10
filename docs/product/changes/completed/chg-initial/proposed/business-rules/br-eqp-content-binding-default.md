---
id: BR-EQP-CONTENT-BINDING-DEFAULT
type: business-rule
title: "Content lines default to reserved from stock"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-DEFAULT-CONTENT"
  - "UC-EQP-DEFINE-PHYSICAL-KIT"
uses-terms:
  - "TERM-CONTENT-STOCK-BINDING"
provenance:
  source: ".private/research/help-center/articles/19895983469330.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Each new default content line of a physical kit is reserved from stock unless the user marks it available outside the kit; lines that existed before the setting was introduced also behave as reserved from stock.

## Rationale

Preserves the established fixed behaviour of cases.

## Examples

- Adding a speaker to a case without changing anything makes it fixed to the case.

## Exceptions

None.
