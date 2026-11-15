---
id: "UC-DOC-SHOW-EQUIPMENT-IMAGES"
type: use-case
title: "Show equipment images on documents"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-EQUIPMENT-IMAGE-FLAG"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014240919.md, .private/research/help-center/articles/360014405639.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Equipment lines carry pictures of the articles.

## Trigger

Customers want to see what they rent.

## Preconditions

Articles have uploaded images.

## Main Flow

1. The user marks an article image to be displayed in documents.
2. The administrator chooses to show equipment images on the document types that list equipment.
3. The administrator saves.

## Alternative Flows

None.

## Failure Conditions

Articles without a flagged image show no picture.

## Postconditions

Generated documents show the flagged images.
