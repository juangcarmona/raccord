---
id: "UC-DOC-SAVE-TEXT-TEMPLATE"
type: use-case
title: "Save an e-mail or personal text template"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-TEXT-TEMPLATE-PER-TYPE"
  - "BR-DOC-TEXT-TEMPLATE-VISIBILITY"
uses-terms:
  - "TERM-TEXT-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/360014260839.md, .private/research/help-center/articles/360014391919.md, .private/research/help-center/articles/360014405639.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Recurring wording is reusable.

## Trigger

The user has written a message or personal text worth reusing.

## Preconditions

The user is composing the e-mail or personal text of a document.

## Main Flow

1. The user writes the text (with images).
2. The user saves it as a template with a name.
3. The user chooses who may see it.
4. The product stores it for the document's type.

## Alternative Flows

None.

## Failure Conditions

- Saving without a name or without any text is refused with the reason and nothing is saved.
- A saved template is offered only when composing for documents of the type it was saved for, so it does not appear elsewhere (BR-DOC-TEXT-TEMPLATE-PER-TYPE).
- Users outside the chosen visibility are never offered the template (BR-DOC-TEXT-TEMPLATE-VISIBILITY).
- An actor without permission to save templates is refused and nothing changes.

## Postconditions

The template is offered when composing for documents of that type.
