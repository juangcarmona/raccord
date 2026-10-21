---
id: UC-FIN-CONFIGURE-VAT
type: use-case
title: "Configure VAT classes and schemes"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-VAT-FROM-CLASS-AND-SCHEME"
  - "BR-FIN-USED-RATE-FROZEN"
  - "BR-FIN-DEFAULT-VAT-CLASS"
uses-terms:
  - "TERM-VAT-CLASS"
  - "TERM-VAT-SCHEME"
  - "TERM-VAT-SCHEME-KIND"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648540.md, .private/research/help-center/articles/360017648360.md, .private/research/help-center/articles/360015935839.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Every priced item is taxed at the correct rate.

## Trigger

Setting up, or a legal rate change.

## Preconditions

None.

## Main Flow

1. The administrator reviews the preconfigured classes and schemes of the country.
2. The administrator creates classes and schemes (per class, fixed rate or reverse charge).
3. The administrator sets the default scheme and default classes for new items.
4. The administrator assigns classes to items and functions, in bulk if needed.

## Alternative Flows

Yearly rate change: a new scheme named after the year becomes the default; old schemes are kept.

## Failure Conditions

Editing a used rate is refused.

## Postconditions

Projects compute VAT from classes and schemes.
