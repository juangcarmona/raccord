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
    source: ".private/research/help-center/articles/12687358328338.md, .private/research/help-center/articles/31926136198546.md; issue #307"
    confidence: high
    recovered-from: documentation
---

## Goal

The item is visually recognisable.

## Trigger

An item lacks a picture or needs a better one.

## Preconditions

- The item exists.
- The equipment manager may maintain its images.

## Main Flow

1. The equipment manager chooses to set an image.
2. The manager uploads an image, or picks one from a web image search.
3. For a direct upload, the product checks the actual file type and size against the configured Equipment image limits.
4. The product stores an accepted image through the document-store capability and shows it in the item details.

## Alternative Flows

- 2a. The manager adds an image as an ordinary file; its applicability to the Equipment image limits is an open question.
- 3a. The manager picks an image from web search; its applicability to the same limits is an open question.

## Failure Conditions

- The uploaded file type is not allowed or the file exceeds the configured maximum size: the product refuses it before durable storage and leaves the current image and item data unchanged.
- Images in import files: not supported; images are added one by one.

## Postconditions

- The item has an accepted image among its files.
- The first image that is not marked confidential remains the default image.
