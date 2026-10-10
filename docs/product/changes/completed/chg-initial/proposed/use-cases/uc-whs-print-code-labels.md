---
id: UC-WHS-PRINT-CODE-LABELS
type: use-case
title: "Generate and print code labels"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by: []
uses-terms:
  - "TERM-SCAN-CODE"
  - "TERM-COMBINATION-LABEL"
provenance:
  source: ".private/research/help-center/articles/360013101260.md, .private/research/help-center/articles/5843706612370.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Labels exist for selected equipment.

## Trigger

Labelling.

## Preconditions

None.

## Main Flow

1. The technician selects articles, a template, letterhead (defines label size), and generates.
2. The document can be saved to the article.

## Alternative Flows

- Case label with content.

## Failure Conditions

- Generating with no articles selected, or without a template or letterhead, is refused with the reason and nothing is produced.
- A label size defined by the letterhead that cannot hold the code is refused with the reason.
- If rendering fails, nothing is stored with the article and generation can be retried.
- An actor without permission to generate documents is refused and nothing changes.

## Postconditions

Labels printed.
