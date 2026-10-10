---
id: BR-FIN-ONLINE-VERSION-VISIBILITY
type: business-rule
title: "What the customer sees depends on the regeneration mode"
status: draft
applies-to:
  - "UC-FIN-REGENERATE-QUOTE"
  - "UC-FIN-PUBLISH-ONLINE-QUOTE"
uses-terms:
  - "TERM-ONLINE-QUOTE-STATUS"
  - "TERM-REGENERATION-MODE"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Rule

For a published quote, overwriting and creating a new version are visible to the customer at once through the same link, whereas a new version created without synchronising stays visible only to the company until it is synchronised; earlier versions remain accessible to both sides.

## Rationale

Allows previewing changes before showing them.

## Examples

See SB-FIN-UNSYNCED-VERSION-HIDDEN.

## Exceptions

None.
