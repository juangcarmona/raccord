---
id: UC-FIN-CONFIGURE-PRICE-FACTORS
type: use-case
title: "Configure price factor groups"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-PRICE-FACTOR-BY-DURATION"
  - "BR-FIN-DEFAULT-FACTOR-GROUP"
uses-terms:
  - "TERM-FACTOR-GROUP"
  - "TERM-PRICE-FACTOR"
provenance:
  source: ".private/research/help-center/articles/360013944139.md, .private/research/help-center/articles/360015935839.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Multi-day rentals are priced automatically by duration.

## Trigger

The company defines or changes its duration pricing.

## Preconditions

None.

## Main Flow

1. The administrator edits the default factor group, defining day ranges and factors.
2. The administrator adds further factor groups for different equipment types.
3. The administrator assigns factor groups to equipment items, individually or in bulk.

## Alternative Flows

None.

## Failure Conditions

- Overlapping or gapped day ranges, or a factor that is not a positive number, are refused with the reason and nothing is saved (BR-FIN-PRICE-FACTOR-BY-DURATION).
- A usage period longer than any defined range yields no factor; the group's definition must cover it or the price is not adjusted.
- An actor without permission to configure pricing is refused and nothing changes.
- If another user changed the same group or items in the meantime, the change is refused and the user must reload and retry.

## Postconditions

Planned equipment is priced with the factor of its group.
