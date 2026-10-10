---
id: SB-FIN-NEW-YEAR-VAT-SCHEME
type: structured-behaviour
title: "Rate change via a new scheme"
status: draft
illustrates:
  - "BR-FIN-USED-RATE-FROZEN"
given:
  - "The current scheme with low rate 10% has been used"
when: "The administrator creates scheme \"VAT 2027\" with low rate 15% and makes it default"
then:
  - "New projects use 15% on low-rate items"
  - "Projects using the old scheme keep their rates"
  - "Editing the used low rate of the old scheme is refused"
uses-terms:
  - "TERM-VAT-SCHEME"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648360.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Historic accuracy.

## Boundaries

None.
