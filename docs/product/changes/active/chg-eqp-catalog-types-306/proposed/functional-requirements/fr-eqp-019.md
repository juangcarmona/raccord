---
id: FR-EQP-019
type: functional-requirement
title: "Browse the Equipment catalog in pages"
status: draft
derived-from:
    - "UC-EQP-BROWSE-CATALOG"
verification:
    - scenario: "The manager can continue through every catalog page in stable order without duplicate or missing items."
uses-terms:
    - "TERM-EQUIPMENT-ITEM"
provenance:
    source: "Issue #306"
    confidence: high
    recovered-from: inference
---

## Requirement

The product MUST return the equipment catalog in bounded pages with a stable order, provide a continuation path when more items remain, and indicate when no further page exists. Following the continuation path MUST NOT repeat or omit catalog items while the catalog remains unchanged.

## Rationale

Paged browsing supports large catalogs while keeping the result bounded and navigable.
