---
id: "SB-DOC-MISMATCHED-LETTERHEAD-SIZE"
type: structured-behaviour
title: "A wrongly sized letterhead is misplaced"
status: draft
illustrates:
  - "BR-DOC-LETTERHEAD-PAGE-SIZE"
given:
  - "A letterhead file is Letter size but is declared as A4"
when: "A document is generated on the letterhead"
then:
  - "The background is not aligned with the page"
  - "Correcting the declared size to Letter aligns it"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

The declared size must match the file.

## Boundaries

None.
