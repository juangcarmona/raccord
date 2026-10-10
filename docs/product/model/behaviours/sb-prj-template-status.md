---
id: SB-PRJ-TEMPLATE-STATUS
type: structured-behaviour
title: "A template-based project starts in the template's status"
status: draft
illustrates:
  - "BR-PRJ-INITIAL-STATUS"
given:
  - "A template was saved from a project in status Confirmed"
when: "A coordinator creates a project from that template"
then:
  - "The new project starts in status Confirmed"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/360013367659.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Templates can set a different starting status.

## Boundaries

None.
