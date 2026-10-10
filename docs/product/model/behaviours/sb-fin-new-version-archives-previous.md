---
id: SB-FIN-NEW-VERSION-ARCHIVES-PREVIOUS
type: structured-behaviour
title: "New version keeps the previous one"
status: draft
illustrates:
  - "BR-FIN-REGENERATION-MODES"
given:
  - "Quote Q-120 is at version 2"
when: "The coordinator regenerates it as a new version"
then:
  - "The quote number is unchanged"
  - "The version becomes 3"
  - "Version 2 is stored as a project file"
uses-terms:
  - "TERM-QUOTE-VERSION"
  - "TERM-REGENERATION-MODE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Revision history is preserved.

## Boundaries

None.
