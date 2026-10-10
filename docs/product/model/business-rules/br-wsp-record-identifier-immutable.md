---
id: BR-WSP-RECORD-IDENTIFIER-IMMUTABLE
type: business-rule
title: "Record identifiers never change"
status: draft
applies-to:
  - "UC-WSP-UPDATE-BY-IMPORT"
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-RECORD-IDENTIFIER"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A record identifier is assigned when the record is created and can never be changed by any user, including administrators.

## Rationale

It is the permanent anchor that makes imports and integrations reliable.

## Examples

- Renaming an item or changing its code leaves its identifier unchanged.

## Exceptions

None.
