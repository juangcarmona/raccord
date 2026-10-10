---
id: TERM-API-ITEM-REFERENCE
type: domain-term
title: "API item reference"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/15274709111826.md, .private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The relative path that identifies one record in the public interface (kind plus numeric identifier, for example a contact or a task under its project). Combined with the interface base address it retrieves the record. Records that hang under another record also carry the reference of that parent.

## Distinguish From

Not the record's business number (project number, contact number).

## Usage

Returned in collections and in parent-path fields of files and invoice lines.
