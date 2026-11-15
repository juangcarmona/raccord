---
id: UC-PRJ-DELETE-PROJECT
type: use-case
title: "Delete a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-DELETION-PERMANENT"
  - "BR-PRJ-DELETE-BOOKED-WARNING"
  - "BR-PRJ-TEMPLATE-INDEPENDENT"
uses-terms:
  - "TERM-PROJECT"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/360013367659.md, .private/research/help-center/articles/11003249381266.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An unwanted project is removed permanently.

## Trigger

A project was created by mistake or only to build a template.

## Preconditions

The project exists.

## Main Flow

1. The coordinator asks to delete the project.
2. The product warns when the project has equipment booked out that does not fit its status.
3. The coordinator confirms; the product removes the project for good.

## Alternative Flows

- Cancelling is the recommended alternative because it keeps history.
- A project with an issued invoice, audit event, compliance record, payment or legal workday data cannot be permanently deleted and may only be cancelled or archived.

## Failure Conditions

- Deleting a project with equipment booked out produces a warning that it does not fit the status; without confirmation, nothing is deleted (BR-PRJ-DELETE-BOOKED-WARNING).
- A project with legally relevant or auditable records is refused for permanent deletion and the product explains that it must be cancelled or archived.
- A deleted project cannot be recovered (BR-PRJ-DELETION-PERMANENT).
- An actor without permission to delete projects is refused and nothing changes.
- If another user changed the project in the meantime, the deletion is refused and the coordinator must reload and retry.

## Postconditions

The project no longer exists when permanent deletion is allowed; templates made from it keep working.
