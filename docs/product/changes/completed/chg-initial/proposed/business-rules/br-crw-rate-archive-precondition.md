---
id: BR-CRW-RATE-ARCHIVE-PRECONDITION
type: business-rule
title: "A rate in use on future work cannot be archived"
status: draft
applies-to:
  - "UC-CRW-ARCHIVE-CREW-RATE"
uses-terms:
  - "TERM-CREW-RATE-STATUS"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A crew rate can only be archived when no crew member is linked to it and no future function or shift uses it; past functions keep the archived rate and show it struck through. Archived rates are listed separately and are not selectable.

## Rationale

Keeps history and financial statistics intact while stopping new use.

## Examples

A 2024 rate still used on a function next month must first be replaced on that function.

## Exceptions

None.
