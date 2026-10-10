---
id: UC-EQP-IMPORT-KIT-CONTENT
type: use-case
title: "Import default content of kits"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-CONTENT-IMPORT-MODES"
uses-terms:
  - "TERM-DEFAULT-CONTENT"
  - "TERM-IMPORT-MODE"
  - "TERM-PHYSICAL-KIT"
  - "TERM-VIRTUAL-KIT"
provenance:
  source: ".private/research/help-center/articles/20045646271250.md, .private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Kit content is filled for many kits at once.

## Trigger

Items and kits have been imported and their content must be defined.

## Preconditions

- The kits and their content items exist in the catalog.

## Main Flow

1. The equipment manager starts a content import and chooses update or overwrite.
2. The manager downloads the generated sample file, which lists existing kits, catalog items and current content, and pairs kits with content and quantities.
3. The manager uploads the file, confirms the header row and column mapping, and checks the preview.
4. The product imports and shows the result.

## Alternative Flows

- 1a. Overwrite: content lines not in the file are deleted from those kits.

## Failure Conditions

- Failed rows are listed in the result.

## Postconditions

- Kits have the imported default content.
