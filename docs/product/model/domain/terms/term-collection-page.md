---
id: TERM-COLLECTION-PAGE
type: domain-term
title: "Collection page"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-PAGINATION-CURSOR"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Definition

One response of a collection request: up to the requested limit of records (default 300, at most 1500) plus, in cursor mode, the address of the next page, which is empty on the last page. In offset mode, used when sorting by another column than the identifier, no next-page address is given and the caller computes offsets.

## Distinguish From

Not the whole collection; not a filter.

## Usage

Every list call of the public interface returns a page.
