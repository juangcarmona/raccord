---
id: UC-FIN-APPLY-CUSTOMER-DEFAULTS
type: use-case
title: "Apply customer defaults to a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-CUSTOMER-DEFAULTS-ON-SELECTION"
  - "BR-FIN-DEFAULTS-NOT-RETROACTIVE"
  - "BR-FIN-DISCOUNT-GROUP-PERCENTAGE"
  - "BR-FIN-VAT-SCHEME-PRECEDENCE"
  - "BR-FIN-MOMENT-DEFAULTS"
uses-terms:
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013890780.md, .private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A project uses the client's agreed discounts, invoice moment, payment term and taxation.

## Trigger

A contact is selected as client or location of a project.

## Preconditions

The contact has financial defaults.

## Main Flow

1. The coordinator selects the client.
2. The product asks whether to apply the client's defaults.
3. On confirmation, the product copies discounts, invoice moment, payment term and VAT scheme or tax rate and recalculates.

## Alternative Flows

Updated discount-group percentages: the coordinator updates the equipment data of the project to refresh line discounts. Updated category discounts: the coordinator removes and re-selects the client.

## Failure Conditions

The coordinator declines: nothing is copied.

## Postconditions

The project carries the client's conditions.
