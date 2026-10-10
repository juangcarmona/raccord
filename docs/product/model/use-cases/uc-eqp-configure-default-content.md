---
id: UC-EQP-CONFIGURE-DEFAULT-CONTENT
type: use-case
title: "Set behaviour of kit content lines"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-CONTENT-BINDING-DEFAULT"
  - "BR-EQP-CONTENT-DOCUMENT-VISIBILITY"
  - "BR-WHS-KIT-CONTENT-EDITABILITY"
uses-terms:
  - "TERM-DEFAULT-CONTENT"
  - "TERM-CONTENT-STOCK-BINDING"
  - "TERM-PHYSICAL-KIT"
provenance:
  source: ".private/research/help-center/articles/19895983469330.md, .private/research/help-center/articles/20641311900690.md, .private/research/help-center/articles/20641257383954.md, .private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/20084575989010.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Each content line of a kit behaves as intended for stock and documents.

## Trigger

The manager refines how a kit's content should behave.

## Preconditions

- The kit has default content.

## Main Flow

1. The equipment manager opens the kit's default content.
2. For each line the manager chooses reserved from stock or available outside the kit.
3. For each line the manager chooses whether it is shown on quotes and on packing lists.
4. The product saves the settings and applies them to stock and documents.

## Alternative Flows

- 3a. When generating a document, the user may override the per-line choice by showing or hiding all content.

## Failure Conditions

- Settings for stock binding are not offered for virtual kits.

## Postconditions

- Content lines carry explicit behaviour; mixed fixed and flexible content is possible within one kit.
