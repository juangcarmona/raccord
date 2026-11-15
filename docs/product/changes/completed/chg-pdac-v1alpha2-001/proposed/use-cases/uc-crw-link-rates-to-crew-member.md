---
id: UC-CRW-LINK-RATES-TO-CREW-MEMBER
type: use-case
title: "Link rates to crew members"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PRICE-RATES-ON-FUNCTIONS-ONLY"
  - "BR-CRW-ONE-DEFAULT-RATE"
  - "BR-CRW-DAILY-RATE-MAXIMUM-HOURS"
uses-terms:
  - "TERM-CREW-RATE"
  - "TERM-RATE-SCOPE"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md, .private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Each crew member has the cost rates for their skills and a default.

## Trigger

A person's pay terms are set.

## Preconditions

Global cost rates exist, or an individual rate is defined.

## Main Flow

1. The planner opens the crew member's rates.
2. The planner links global rates or creates an individual rate with factors, or a simple rate with hourly, daily and maximum hours.
3. The planner makes one rate default.
4. The product saves.

## Alternative Flows

Bulk: the planner selects several crew members, links one global cost rate and optionally makes it default.

## Failure Conditions

Price rates cannot be linked.

## Postconditions

The crew member's default rate applies when planned.
