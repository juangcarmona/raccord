---
id: "SB-DOC-LETTERHEAD-FIRST-PAGE-ONLY"
type: structured-behaviour
title: "A blank last page limits the letterhead to the first page"
status: draft
illustrates:
  - "BR-DOC-LETTERHEAD-PAGE-MAPPING"
given:
  - "A letterhead PDF has the company design on page 1 and a blank page 2"
when: "A three-page quotation is generated on it"
then:
  - "Page 1 shows the company design"
  - "Pages 2 and 3 are plain"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360017648260.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows how to brand only the first page.

## Boundaries

None.
