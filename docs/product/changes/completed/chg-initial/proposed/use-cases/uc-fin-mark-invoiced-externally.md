---
id: UC-FIN-MARK-INVOICED-EXTERNALLY
type: use-case
title: "Mark a project as invoiced externally"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-EXTERNAL-INVOICING"
uses-terms:
  - "TERM-EXTERNALLY-INVOICED"
provenance:
  source: ".private/research/help-center/articles/25948227210258.md, .private/research/help-center/articles/360013628279.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Work billed outside the product stops appearing as to be invoiced.

## Trigger

The invoice was produced by other software or an accountant.

## Preconditions

The project is in the invoicing queue.

## Main Flow

1. The administrator selects entries and marks them invoiced externally, confirming.
2. The product moves them to the externally invoiced list.

## Alternative Flows

None.

## Failure Conditions

- Entries that are not in the invoicing queue cannot be marked; the request is refused with the reason (BR-FIN-EXTERNAL-INVOICING).
- Without the confirmation, nothing is marked and the entries stay in the queue.
- An actor without permission to manage invoicing is refused and nothing changes.
- If another user already invoiced or marked the same entry in the meantime, the change is refused and the user must reload and retry.

## Postconditions

No further reminder for those entries.
