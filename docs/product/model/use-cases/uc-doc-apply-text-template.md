---
id: "UC-DOC-APPLY-TEXT-TEMPLATE"
type: use-case
title: "Start a message from a text template"
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
  source: ".private/research/help-center/articles/360014260839.md, .private/research/help-center/articles/29667214920082.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A message or personal text is prepared quickly.

## Trigger

The user composes an e-mail or personal text for a document.

## Preconditions

Text templates for this document type are visible to the user.

## Main Flow

1. The user picks a template from the list.
2. The product fills the text field.
3. The user adjusts the text.

## Alternative Flows

None.

## Failure Conditions

A template saved for another document type is not listed.

## Postconditions

The text is ready for sending or generation.
