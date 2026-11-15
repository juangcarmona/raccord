---
id: UC-FIN-CONFIGURE-INVOICE-MOMENTS
type: use-case
title: "Configure invoice moments"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-MOMENT-TRIGGER-DATES"
  - "BR-FIN-MOMENT-PERCENTAGES"
  - "BR-FIN-MOMENT-DEFAULTS"
uses-terms:
  - "TERM-INVOICE-MOMENT"
  - "TERM-INVOICE-MOMENT-TRIGGER"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360017647300.md, .private/research/help-center/articles/360015935839.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Projects are billed at agreed milestones.

## Trigger

Setting up invoicing.

## Preconditions

None.

## Main Flow

1. The administrator creates an invoice moment with name, printed name and up to three parts with percentage and trigger.
2. The administrator assigns defaults to project types and customers.

## Alternative Flows

None.

## Failure Conditions

- A moment with no parts, more than three parts, or part percentages that do not add up to the whole total is refused with the reason and nothing is saved (BR-FIN-MOMENT-PERCENTAGES, BR-FIN-MOMENT-TRIGGER-DATES).
- A moment saved without a name is refused with the reason.
- An actor without permission to configure invoicing is refused and nothing changes.
- If another user changed the same moment in the meantime, the change is refused and the user must reload and retry.

## Postconditions

Projects show in the invoicing queue according to their moments.
