---
id: SB-PRJ-CONTACT-IMPORT-SKIP-EXISTING
type: structured-behaviour
title: "Skip-existing import ignores known contacts"
status: draft
illustrates:
  - "BR-PRJ-CONTACT-IMPORT-MODES"
given:
  - "The database has contact 'Acme'"
  - "The file has rows 'Acme' and 'Beta' with name as match column"
when: "The user imports in add-new-and-skip-existing mode"
then:
  - "Only 'Beta' is created"
  - "'Acme' is unchanged"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013553000.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Duplicate prevention.

## Boundaries

None.
