---
id: "UC-DOC-DESIGN-EQUIPMENT-LABEL"
type: use-case
title: "Set up equipment labels"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-LETTERHEAD-PAGE-SIZE"
  - "BR-DOC-LABEL-SAVE-TO-ITEM"
uses-terms:
  - "TERM-EQUIPMENT-LABEL"
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/20257253618322.md, .private/research/help-center/articles/360017700539.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Labels with codes and article details print correctly on the company's label stock.

## Trigger

The company starts tagging equipment.

## Preconditions

The label dimensions are known.

## Main Flow

1. The administrator adds a letterhead with the label's width and height.
2. The administrator chooses that letterhead for the built-in equipment label document type.
3. The administrator chooses which article details and codes the label shows.
4. Test labels are generated and printed to check size and position.

## Alternative Flows

The labels are downloaded as PDF for the printer's own software.

## Failure Conditions

Content too large for the label spills onto the next label; the administrator reduces the content or the code size.

## Postconditions

Warehouse staff can print labels (UC-WHS-PRINT-CODE-LABELS).
