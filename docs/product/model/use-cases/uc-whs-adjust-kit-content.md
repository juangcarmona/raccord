---
id: UC-WHS-ADJUST-KIT-CONTENT
type: use-case
title: "Adjust combination content in a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-KIT-CONTENT-EDITABILITY"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/19895194067730.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A kit's content is tailored for one job.

## Trigger

The job needs a variant of a standard kit.

## Preconditions

The combination allows content editing in projects.

## Main Flow

1. The coordinator adds, removes or changes quantities of content lines in the planned combination.
2. The product recalculates availability for the content.

## Alternative Flows

- Bulk enabling of the editability setting for many catalog combinations.

## Failure Conditions

- Editability off or physical combination: refused.
- Content reserved from stock: cannot be removed or re-quantified.

## Postconditions

The planned content differs from the default content only in this project.
