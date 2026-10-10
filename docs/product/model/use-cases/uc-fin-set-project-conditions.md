---
id: UC-FIN-SET-PROJECT-CONDITIONS
type: use-case
title: "Set the conditions of a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-DEFAULTS-BY-PROJECT-TYPE"
uses-terms:
  - "TERM-ADDITIONAL-CONDITIONS"
  - "TERM-TERMS-AND-CONDITIONS"
provenance:
  source: ".private/research/help-center/articles/360013938099.md, .private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Quotes and invoices of the project carry the right conditions.

## Trigger

A job needs specific conditions, for example for special equipment.

## Preconditions

The project exists.

## Main Flow

1. The product pre-selects the default additional conditions of the project type and includes the general terms.
2. The coordinator selects other saved additional conditions or writes free additional text.
3. The coordinator saves; documents generated afterwards print them.

## Alternative Flows

The document template lacks the additional-conditions section: the text is not printed until the template includes it.

## Failure Conditions

- A condition that no longer exists cannot be selected; the selection is refused with the reason.
- When the template lacks the additional-conditions section, the text is saved but not printed.
- An actor without permission to edit the project is refused and nothing changes.
- If another user changed the project in the meantime, the change is refused and the coordinator must reload and retry (BR-FIN-DEFAULTS-BY-PROJECT-TYPE).

## Postconditions

The project has its conditions.
