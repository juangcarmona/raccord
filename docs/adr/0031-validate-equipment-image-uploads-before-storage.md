---
name: validate-equipment-image-uploads-before-storage
description: Equipment validates uploaded image content before storing it through the generic document-store port.
status: Proposed
date: 2026-10-20
deciders: ["Juan G. Carmona"]
tags: ["equipment", "document-storage", "uploads", "security"]
---

# ADR-0031: Validate Equipment image uploads before storage

## Context

ADR-0009 routes binary files through a generic `IDocumentStore` port with file-system and Azure Blob adapters. Equipment images have product-specific file-type and byte-size limits that the generic document store does not know. Storing an upload before validating it could leave oversized or unsupported content in durable storage.

## Decision

We will validate Equipment image uploads in the Equipment feature before calling `IDocumentStore`. Validation will use the configured allowed file types and maximum byte size chosen by the Owner. Only accepted bytes are passed to the existing document-store port; the storage adapters remain product-agnostic.

On validation failure, the Equipment feature will return a user-visible refusal without writing a document or changing the item's current image. No new storage adapter or production dependency is introduced.

## Consequences

- **Positive:** Product-specific constraints are enforced where the Equipment context and item are known, and rejected files do not create stored objects.
- **Negative:** The Equipment feature must safely inspect file bytes and keep its validation aligned with its configured limits; tests must cover invalid content and storage non-invocation.
- **Neutral:** ADR-0009 remains the durable storage contract. Its accepted record is not rewritten; this ADR adds an Equipment-specific rule at the caller boundary. The exact formats and maximum size remain product decisions under `CHG-EQP-ITEM-IMAGE-LIMITS-307`.

## Alternatives considered

- **Validate only inside document-store adapters:** rejected because the generic storage port has no Equipment semantics and would force the file-system and Azure adapters to duplicate product rules.
- **Trust the filename extension and request content-type:** rejected because caller-supplied metadata does not establish the actual content type.
- **Store first and validate later:** rejected because rejected content would already consume durable storage and could replace a valid item image.

## References

- Issue: [#307](https://github.com/juangcarmona/raccord/issues/307)
- Epic: [#375](https://github.com/juangcarmona/raccord/issues/375)
- Product Change: `CHG-EQP-ITEM-IMAGE-LIMITS-307`
- Related: [ADR-0009](0009-document-storage.md), [ADR-0030](0030-equipment-module-boundary.md)
