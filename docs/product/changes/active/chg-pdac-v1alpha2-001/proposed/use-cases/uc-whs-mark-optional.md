---
id: UC-WHS-MARK-OPTIONAL
type: use-case
title: "Mark planned equipment as optional"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-OPTIONAL-EXCLUDED-FROM-TOTAL"
uses-terms:
  - "TERM-OPTIONAL-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360014284900.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Lines are offered as options without counting in the quotation total.

## Trigger

The customer should choose between alternatives or extras.

## Preconditions

Lines are planned.

## Main Flow

1. The coordinator flags one or several lines as optional.
2. The product excludes them from the total and shows them as options on quotations.

## Alternative Flows

- Separate section: the quotation layout lists options in their own section.

## Failure Conditions

- Flagging lines when none are planned or selected is refused with the reason.
- An actor without permission to edit the plan is refused and nothing changes.
- Optional lines are excluded from the total; lines wrongly left unflagged are still charged (BR-WHS-OPTIONAL-EXCLUDED-FROM-TOTAL).
- If another user changed the same lines in the meantime, the change is refused and the coordinator must reload and retry.

## Postconditions

Options are visible but not totalled.
