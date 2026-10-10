---
id: FR-DOC-101
type: functional-requirement
title: "Carry images in e-mails, texts and signatures"
status: draft
derived-from:
  - "UC-DOC-INSERT-IMAGE"
verification:
  - id: "INSERT-IMAGE"
    scenario: "Given an e-mail, personal text or signature being edited, when the user uploads an image and resizes it, the image becomes part of the text and appears in the sent message at the chosen size."
uses-terms:
  - "TERM-PERSONAL-TEXT"
  - "TERM-EMAIL-SIGNATURE"
provenance:
  source: ".private/research/help-center/articles/360014405639.md, .private/research/help-center/articles/360014391919.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a user insert an uploaded image into an e-mail, a personal text or a signature, and MUST let the user resize it, so that the image is part of the text wherever that text is rendered or sent. Equipment images MUST remain managed through the equipment catalog rather than through text editing.

## Rationale

Logos and illustrations make messages recognisable to the recipient.
