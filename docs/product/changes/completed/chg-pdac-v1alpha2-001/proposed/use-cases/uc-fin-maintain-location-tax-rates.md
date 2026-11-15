---
id: UC-FIN-MAINTAIN-LOCATION-TAX-RATES
type: use-case
title: "Maintain location-based tax rates"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-RACCORD-AUTOMATION"
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-LOCATION-RATE-UPDATES"
  - "BR-FIN-TAXABLE-FLAG"
uses-terms:
  - "TERM-LOCATION-TAX-RATE"
provenance:
  source: ".private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Sales tax is charged at the current local rate.

## Trigger

The company operates in a location-based sales tax system.

## Preconditions

None.

## Main Flow

1. The administrator adds a tax rate by searching a postal code and naming it.
2. The product fills in the current rate and keeps it updated monthly with history.
3. The administrator marks equipment as taxable or not and assigns rates to customers.

## Alternative Flows

An existing manual rate is linked to a postal code to receive updates.

## Failure Conditions

- A postal code that matches no location is refused with the reason and no rate is added.
- If the rate source cannot be reached, the current rate is kept, the failure is visible and the update is retried (BR-FIN-LOCATION-RATE-UPDATES).
- A rate change takes effect at the next monthly update, not mid-month (BR-FIN-LOCATION-RATE-UPDATES).
- An actor without permission to configure taxes is refused and nothing changes.

## Postconditions

Rates stay current and projects show a notice when a rate changes.
