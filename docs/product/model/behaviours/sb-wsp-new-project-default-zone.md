---
id: SB-WSP-NEW-PROJECT-DEFAULT-ZONE
type: structured-behaviour
title: "New project starts in the default zone"
status: draft
illustrates:
  - "BR-WSP-DEFAULT-TIME-ZONE-BASELINE"
given:
  - "The workspace default time zone is Europe/Madrid"
  - "Multiple time zones are enabled"
when: "A planner creates a project"
then:
  - "The project's time zone is the workspace default"
uses-terms:
  - "TERM-WORKSPACE-TIME-ZONE"
provenance:
  source: ".private/research/help-center/articles/115003579769.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Baseline zone.

## Boundaries

None.
