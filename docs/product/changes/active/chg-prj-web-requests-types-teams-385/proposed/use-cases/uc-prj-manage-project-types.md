---
id: UC-PRJ-MANAGE-PROJECT-TYPES
type: use-case
title: "Define project types"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-PROJECT-TYPE-DEFAULTS"
  - "BR-PRJ-REMEMBER-LAST-TEMPLATE"
uses-terms:
  - "TERM-PROJECT-TYPE"
  - "TERM-PROJECT-CATEGORY"
provenance:
  source: ".private/research/help-center/articles/360013631800.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Projects start with the right settings and can be told apart in overviews and statistics.

## Trigger

The company distinguishes kinds of jobs (dry hire, sale, festival).

## Preconditions

Administrator rights.

## Main Flow

1. The administrator adds a project type with name, category and colour.
2. The administrator sets default document templates (each optionally 'remember last used'), letterhead, number series for quotations, contracts, invoices and credit notes, invoice moment and additional conditions.
3. The product saves the type for use on projects.

## Alternative Flows

None.

## Failure Conditions

- A type without a name or category is refused with the reason and nothing is saved.
- An actor without administrator rights is refused and nothing changes.
- A reference to a template, letterhead or number series that no longer exists is refused with the reason (BR-PRJ-PROJECT-TYPE-DEFAULTS).
- If another administrator changed the same type in the meantime, the change is refused and the administrator must reload and retry.

## Postconditions

New projects of the type receive its defaults.
