---
id: FR-EQP-020
type: functional-requirement
title: "Validate Equipment image files before storage"
status: draft
derived-from:
    - "UC-EQP-ADD-ITEM-IMAGE"
    - "CON-EQP-IMAGE-UPLOAD-LIMITS"
verification:
    - scenario: "An image with an allowed type and size is stored and shown on its item."
    - scenario: "A disallowed or oversized image is rejected, no document is stored, and the item's current image remains unchanged."
uses-terms:
    - "TERM-EQUIPMENT-IMAGE"
provenance:
    source: "Issue #307"
    confidence: high
    recovered-from: inference
---

## Requirement

The product MUST validate an uploaded Equipment image against configured file-type and byte-size limits before storing it through the document-store capability. If either limit is exceeded, the product MUST refuse the upload without storing the file or changing the item's current image or other data.

## Rationale

Validating before durable storage prevents unsupported or oversized files from consuming document storage and keeps a failed upload from corrupting the catalog item's image state.
