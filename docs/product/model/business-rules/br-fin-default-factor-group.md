---
id: BR-FIN-DEFAULT-FACTOR-GROUP
type: business-rule
title: "Items without an assigned factor group use the default group"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-PRICE-FACTORS"
uses-terms:
  - "TERM-FACTOR-GROUP"
provenance:
  source: ".private/research/help-center/articles/360013944139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An equipment item uses the factor group assigned to it, otherwise the default factor group.

## Rationale

Different equipment types can follow different price curves.

## Examples

Rigging assigned to a rigging group uses that table; audio without assignment uses the default table.

## Exceptions

None.
