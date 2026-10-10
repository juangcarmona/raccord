---
id: SB-INT-PAGE-LIMIT-CAPPED
type: structured-behaviour
title: "Requested page size above the maximum"
status: draft
illustrates:
  - "BR-INT-PAGE-SIZE-LIMITS"
given:
  - "the workspace has 4000 contacts"
when: "an application requests contacts with a limit of 5000"
then:
  - "the first page contains 1500 contacts"
  - "the page carries the address of the next page"
uses-terms:
  - "TERM-COLLECTION-PAGE"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows the maximum page size.

## Boundaries

Whether an over-limit value is refused or capped is not stated in the source; capping is assumed (medium certainty).
