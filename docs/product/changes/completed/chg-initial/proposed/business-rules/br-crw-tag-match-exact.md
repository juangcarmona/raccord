---
id: BR-CRW-TAG-MATCH-EXACT
type: business-rule
title: "Skill matching requires identical tags"
status: draft
applies-to:
  - "UC-CRW-FIND-CREW-WITH-LISTS"
uses-terms:
  - "TERM-SKILL-TAG"
provenance:
  source: ".private/research/help-center/articles/12866138319506.md, .private/research/help-center/articles/14232209807506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A crew member appears in the matching-tags list of a function only if they carry a tag identical to one of the function's tags; when neither functions nor crew have tags, every crew member is listed.

## Rationale

Simple predictable matching.

## Examples

Function tag rigger matches crew tag rigger, not riggers.

## Exceptions

None.
