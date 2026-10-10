---
id: "SB-DOC-LETTERHEAD-SHORTER-THAN-DOCUMENT"
type: structured-behaviour
title: "Pages beyond the letterhead reuse its last page"
status: draft
illustrates:
  - "BR-DOC-LETTERHEAD-PAGE-MAPPING"
given:
  - "A letterhead has two pages"
when: "A four-page invoice is generated on that letterhead"
then:
  - "Page 1 uses letterhead page 1"
  - "Pages 2, 3 and 4 use letterhead page 2"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Continuation pages need no extra letterhead pages.

## Boundaries

None.
