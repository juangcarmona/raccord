---
id: SB-CRW-DEFAULT-CHANGE-NOT-PROPAGATED
type: structured-behaviour
title: "Editing a default function leaves projects unchanged"
status: draft
illustrates:
  - "BR-CRW-DEFAULT-FUNCTION-COPY"
given:
  - "Two projects use a function added from a default"
when: "The default function's price is raised"
then:
  - "Existing projects keep the old function price"
uses-terms:
  - "TERM-DEFAULT-FUNCTION"
provenance:
  source: ".private/research/help-center/articles/34615607831314.md, .private/research/help-center/articles/14232209807506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Quotes stay stable.

## Boundaries

None.
