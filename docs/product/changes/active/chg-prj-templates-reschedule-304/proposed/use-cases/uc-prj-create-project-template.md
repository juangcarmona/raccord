---
id: UC-PRJ-CREATE-PROJECT-TEMPLATE
type: use-case
title: "Create a project template"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-TEMPLATE-CONTENT"
  - "BR-PRJ-TEMPLATE-NOT-EDITABLE"
  - "BR-PRJ-TEMPLATE-INDEPENDENT"
  - "BR-PRJ-TEMPLATE-TASKS"
uses-terms:
  - "TERM-PROJECT-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/360013367659.md, .private/research/help-center/articles/360013763199.md, .private/research/help-center/articles/360013703139.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A reusable starting point exists for a recurring kind of job.

## Trigger

A kind of project recurs or takes long to set up.

## Preconditions

A project exists that represents the kind of job.

## Main Flow

1. The administrator names a new template and selects the project to copy.
2. The product copies the project's reusable content into the template.
3. Optionally the source project is deleted.

## Alternative Flows

- Updating a template: create a project from it, edit, save as a new template, delete the old one.

## Failure Conditions

- A template without a name, or without a source project, is refused with the reason and nothing is created.
- A template cannot be edited directly; a change attempt is refused and a new template must be saved from an adjusted project (BR-PRJ-TEMPLATE-NOT-EDITABLE).
- Crew members and vehicles assigned to functions are not copied, so projects made from the template start unassigned (BR-PRJ-TEMPLATE-CONTENT).
- An actor without permission to manage templates is refused and nothing changes.

## Postconditions

The template is available when creating projects.
