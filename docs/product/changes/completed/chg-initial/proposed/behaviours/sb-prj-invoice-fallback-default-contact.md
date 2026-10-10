---
id: SB-PRJ-INVOICE-FALLBACK-DEFAULT-CONTACT
type: structured-behaviour
title: "Invoice recipient falls back to default contact person"
status: draft
illustrates:
  - "BR-PRJ-INVOICE-RECIPIENT"
given:
  - "Contact Beta has default contact person Sam and no administrative contact person"
when: "An invoice for Beta is sent"
then:
  - "Sam is preselected as recipient"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fallback.

## Boundaries

None.
