---
id: "UC-DOC-STYLE-TEMPLATE"
type: use-case
title: "Brand the document templates"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by: []
uses-terms:
  - "TERM-DOCUMENT-STYLE"
provenance:
  source: ".private/research/help-center/articles/18380229218194.md, .private/research/help-center/articles/360014232559.md, .private/research/help-center/articles/360017699079.md, .private/research/help-center/articles/4405074080658.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Documents match the company's visual identity.

## Trigger

The default look does not fit the brand.

## Preconditions

None.

## Main Flow

1. The administrator opens the branding settings.
2. The administrator uploads the company logo and chooses the colours.
3. The administrator sets the standard texts printed on documents.
4. The administrator saves.

## Alternative Flows

The administrator also chooses a letterhead for the document types (UC-DOC-ADD-LETTERHEAD).

## Failure Conditions

A logo that is not a supported image is refused with the reason and the previous logo remains.

## Postconditions

Documents generated afterwards carry the new branding.
