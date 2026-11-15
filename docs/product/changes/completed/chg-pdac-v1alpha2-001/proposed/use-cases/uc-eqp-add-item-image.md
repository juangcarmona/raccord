---
id: UC-EQP-ADD-ITEM-IMAGE
type: use-case
title: "Add an image to an item"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-DEFAULT-IMAGE"
uses-terms:
  - "TERM-EQUIPMENT-IMAGE"
provenance:
  source: ".private/research/help-center/articles/12687358328338.md, .private/research/help-center/articles/31926136198546.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The item is visually recognisable.

## Trigger

An item lacks a picture or needs a better one.

## Preconditions

- The item exists.

## Main Flow

1. The equipment manager chooses to set an image.
2. The manager uploads an image, or picks one from a web image search.
3. The product stores it among the item's files and shows it in the item details.

## Alternative Flows

- 2a. The manager adds an image as an ordinary file.

## Failure Conditions

- Images in import files: not supported; images are added one by one.

## Postconditions

- The item has a default image.
