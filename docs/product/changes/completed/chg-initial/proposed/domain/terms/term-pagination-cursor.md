---
id: TERM-PAGINATION-CURSOR
type: domain-term
title: "Pagination cursor"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Definition

An opaque position marker embedded in the next-page address of a collection page. Callers follow it as given; they must not construct or alter it.

## Distinguish From

Not a numeric offset.

## Usage

Default paging mode of the public interface.
