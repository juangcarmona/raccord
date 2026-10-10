---
id: "UC-DOC-EDIT-LETTERHEAD"
type: use-case
title: "Adjust a letterhead's options"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-PAGE-NUMBERS"
  - "BR-DOC-LETTERHEAD-MARGINS"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A letterhead prints correctly with or without page numbers.

## Trigger

Generated documents are misaligned or page numbers are (un)wanted.

## Preconditions

The letterhead exists.

## Main Flow

1. The administrator edits the letterhead.
2. The administrator changes dimensions, margins, document types or the page-number option.
3. The administrator saves.

## Alternative Flows

None.

## Failure Conditions

- Dimensions or margins that leave no usable page area for content are refused with the reason and nothing is saved (BR-DOC-LETTERHEAD-MARGINS).
- An actor without permission to edit letterheads is refused and nothing changes.
- If another user changed the same letterhead in the meantime, the save is refused and the administrator must reload and retry.

## Postconditions

Subsequently generated documents use the updated letterhead.
