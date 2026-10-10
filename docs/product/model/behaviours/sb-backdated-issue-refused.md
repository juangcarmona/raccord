---
id: SB-BACKDATED-ISSUE-REFUSED
type: structured-behaviour
title: "An earlier issue date than the last invoice is refused"
status: draft
illustrates:
  - "BR-ISSUE-DATE-ORDER"
given:
  - "The last invoice of series F was issued on 10/06/2026"
when: "Another draft of series F is issued with issue date 09/06/2026"
then:
  - "Nothing is issued"
  - "The refusal reports that the issue date is not accepted by the series"
uses-terms: []
---

## Intent

The same date (10/06/2026) would be accepted.

## Boundaries

None.
