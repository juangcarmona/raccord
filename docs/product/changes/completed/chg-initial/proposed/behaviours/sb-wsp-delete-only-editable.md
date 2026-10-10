---
id: SB-WSP-DELETE-ONLY-EDITABLE
type: structured-behaviour
title: "Delete right limited by edit scope"
status: draft
illustrates:
  - "BR-WSP-DELETE-NEEDS-EDIT"
given:
  - "A role may delete projects"
  - "The same role may change only own projects"
when: "The user tries to delete a project of another account manager"
then:
  - "The deletion is refused"
uses-terms:
  - "TERM-OWN-RECORD"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Deletion never exceeds editing scope.

## Boundaries

None.
