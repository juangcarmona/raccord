---
id: SB-INT-WAREHOUSE-STATUS-WRITE-REFUSED
type: structured-behaviour
title: "Writing prepped to a project is refused"
status: draft
illustrates:
  - "BR-INT-WAREHOUSE-STATUS-NOT-WRITABLE"
when: "an application sets the project status to prepped"
then:
  - "the change is refused"
  - "the project keeps its project-management status"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Separation of status kinds.

## Boundaries

None.
