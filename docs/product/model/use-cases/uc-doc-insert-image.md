---
id: "UC-DOC-INSERT-IMAGE"
type: use-case
title: "Insert an image into an e-mail, text or signature"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by: []
uses-terms:
  - "TERM-PERSONAL-TEXT"
  - "TERM-EMAIL-SIGNATURE"
provenance:
  source: ".private/research/help-center/articles/360014405639.md, .private/research/help-center/articles/360014391919.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Messages carry visuals such as the company logo.

## Trigger

A logo, photo or illustration should accompany the text.

## Preconditions

An e-mail, personal text or signature is being edited.

## Main Flow

1. The user chooses to insert an image.
2. The user uploads or drops an image file and selects it.
3. The user optionally resizes it.

## Alternative Flows

Equipment images on documents follow the images flagged on the articles.

## Failure Conditions

- A file that is not a supported image, or is too large, is refused with the reason and nothing is inserted.
- If the upload cannot be stored, the insertion fails visibly and can be retried.
- An actor without permission to edit the text being composed is refused and nothing changes.

## Postconditions

The image is part of the text.
