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

1. In a new Spanish installation, the administrator reviews the preconfigured standard-rate class and scheme. The scheme starts at 21%, verified against the Agencia Tributaria on 2026-10-21; the value is an editable setup default and does not determine the rate of each operation.
2. The administrator creates additional classes and schemes (per class, fixed rate or reverse charge) needed for the business's operations.
3. The administrator sets the default scheme and default classes for new items.
4. The administrator assigns classes to items and functions, in bulk if needed, based on the applicable treatment.

## Alternative Flows

Rate change: the administrator verifies the current official rate, creates a new scheme with its source and effective-date context, then makes it the default; old schemes are kept.

## Failure Conditions

Editing a used rate is refused.

## Postconditions

Projects compute VAT from classes and schemes.
