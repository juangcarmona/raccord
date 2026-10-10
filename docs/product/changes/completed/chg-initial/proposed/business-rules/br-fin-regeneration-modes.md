---
id: BR-FIN-REGENERATION-MODES
type: business-rule
title: "Regeneration keeps, raises or replaces the document identity"
status: draft
applies-to:
  - "UC-FIN-REGENERATE-QUOTE"
uses-terms:
  - "TERM-REGENERATION-MODE"
  - "TERM-QUOTE-VERSION"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Overwriting keeps the number and version and replaces the content; a new version raises the version number and stores the previous version as a project file; a new quote receives a new number and leaves the old one unchanged.

## Rationale

Distinguishes drafts-in-progress, revisions and genuinely different offers.

## Examples

See SB-FIN-NEW-VERSION-ARCHIVES-PREVIOUS.

## Exceptions

None.
