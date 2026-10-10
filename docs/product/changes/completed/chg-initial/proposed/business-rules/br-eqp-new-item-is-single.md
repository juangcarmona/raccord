---
id: BR-EQP-NEW-ITEM-IS-SINGLE
type: business-rule
title: "New equipment defaults to a single item"
status: draft
applies-to:
  - "UC-EQP-CREATE-ITEM"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
  - "TERM-SINGLE-ITEM"
provenance:
  source: ".private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A newly created equipment item is a single physical item unless the creator chooses physical kit or virtual kit.

## Rationale

Most catalog entries are standalone pieces.

## Examples

- Creating a microphone without touching the structure yields a single item.

## Exceptions

None.
