---
id: UC-CRW-CONFIGURE-INVITATION-TEMPLATES
type: use-case
title: "Configure invitation templates"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-INVITATION-DISCLOSURE"
  - "BR-CRW-INVITATION-TEXT-RESET"
uses-terms:
  - "TERM-INVITATION-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/23925911973010.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Invitations use the company wording and disclose the intended information.

## Trigger

Setting up communication.

## Preconditions

The user may change company configuration.

## Main Flow

1. The administrator sets signature, subject, salutations and greetings.
2. The administrator writes the message per invitation type.
3. The administrator sets which information is shown by default.
4. The administrator previews and saves.

## Alternative Flows

Reset one or all invitation messages to defaults.

## Failure Conditions

- The user lacks the right to change company configuration: the template changes are refused and the current templates stay in force.
- A required value such as the subject is missing or invalid: the templates are refused with the reason and nothing is saved.
- Another administrator changed the templates in the meantime: the save is refused and the user must reload and retry.

## Postconditions

New invitations use the template.
