---
id: SB-CONCURRENT-ISSUING-GAPLESS
type: structured-behaviour
title: "Concurrent issuing keeps numbers gapless"
status: draft
illustrates:
  - "BR-GAPLESS-NUMBERING"
given:
  - "Six valid drafts of series F exist and no invoice of F has been issued in 2026"
when: "All six are issued at the same moment and any refused attempt is retried"
then:
  - "Exactly the numbers F2026-00001 to F2026-00006 are issued"
  - "No number is duplicated or skipped"
uses-terms: []
---

## Intent

Concurrency never breaks numbering.

## Boundaries

None.
