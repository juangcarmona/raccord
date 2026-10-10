---
id: "UC-DOC-REGENERATE-WITH-OTHER-LAYOUT"
type: use-case
title: "Regenerate a document with another template or letterhead"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-LETTERHEAD-PRECEDENCE"
  - "BR-PRJ-DOCUMENT-REGENERATION"
uses-terms:
  - "TERM-GENERATED-DOCUMENT"
  - "TERM-REGENERATION-MODE"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

A shown document is reproduced with a different template or letterhead.

## Trigger

The generated result uses the wrong stationery or layout.

## Preconditions

A document has been generated and is shown.

## Main Flow

1. The user changes the template or letterhead.
2. The user regenerates.
3. The product replaces the shown document with a new rendering.

## Alternative Flows

For quotes and contracts, regeneration modes of the commercial context apply.

## Failure Conditions

- If the chosen template or letterhead is not available for the document's type, regeneration is refused with the reason and the shown document stays unchanged.
- If rendering fails, the shown document is kept as it was and regeneration can be retried.
- Regenerating picks up the current project data, so earlier unsaved wording is replaced by the new rendering (BR-PRJ-DOCUMENT-REGENERATION).
- An actor without permission to generate documents for the project is refused and nothing changes.

## Postconditions

The document reflects the new choices and the record's current data.
