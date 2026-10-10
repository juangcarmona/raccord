---
id: SB-WSP-NO-FINANCE-NO-REVENUE-WIDGET
type: structured-behaviour
title: "Dashboard without financial visibility"
status: draft
illustrates:
  - "BR-WSP-DASHBOARD-RESPECTS-PERMISSIONS"
given:
  - "A user's role lacks financial visibility"
when: "The user adds widgets to their dashboard"
then:
  - "Financial widgets such as revenue charts are not offered"
uses-terms:
  - "TERM-DASHBOARD-WIDGET"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Intent

The dashboard follows module permissions.

## Boundaries

None.
