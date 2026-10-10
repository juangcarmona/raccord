---
id: SB-FIRST-INVOICE-NUMBER
type: structured-behaviour
title: "The first invoice of a series and year"
status: draft
illustrates:
  - "BR-GAPLESS-NUMBERING"
  - "UC-ISSUE-INVOICE"
given:
  - "No invoice of series F has been issued in 2026"
when: "A valid draft of series F is issued with issue date 11/05/2026"
then:
  - "The invoice number is F2026-00001"
  - "The next invoice of series F in 2026 is F2026-00002"
uses-terms: []
---

## Intent

Fixes the number format and the start of each yearly sequence.

## Boundaries

None.
