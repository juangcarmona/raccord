---
id: CON-EQP-IMAGE-UPLOAD-LIMITS
type: constraint
title: "Equipment image uploads obey configured type and size limits"
status: draft
applies-to:
    - "UC-EQP-ADD-ITEM-IMAGE"
uses-terms:
    - "TERM-EQUIPMENT-IMAGE"
provenance:
    source: "Issue #307"
    confidence: high
    recovered-from: inference
---

## Constraint

Before an uploaded Equipment image is stored, its actual file type MUST be on the configured allowlist and its byte size MUST NOT exceed the configured maximum. A failed check MUST refuse the upload before durable storage and leave the item's current image and other data unchanged. The allowlist, maximum size and whether this constraint also covers web-search images or ordinary attachments require an Owner decision.

## Rationale

Equipment image uploads must use bounded storage and reject content outside the supported image formats.

## Consequences

- The product must expose or document the configured limits so an Equipment manager can understand a refusal.
- The Equipment feature must check content before calling the generic document-store port.
- A rejected upload must not create an orphaned stored object or replace an existing image.
