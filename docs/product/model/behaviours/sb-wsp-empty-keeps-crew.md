---
id: SB-WSP-EMPTY-KEEPS-CREW
type: structured-behaviour
title: "Emptying keeps crew and roles"
status: draft
illustrates:
  - "BR-WSP-EMPTY-DATA-SCOPE"
  - "BR-WSP-EMPTY-SIGNS-OUT"
given:
  - "The evaluation workspace contains sample data and the company's crew members"
when: "The administrator empties the evaluation workspace"
then:
  - "All users are signed out during the process"
  - "Projects, equipment, contacts, tags and custom fields are deleted"
  - "Crew members, roles, settings and document templates remain"
uses-terms:
  - "TERM-SAMPLE-DATA"
provenance:
  source: ".private/research/help-center/articles/360013982380.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Scope of emptying.

## Boundaries

None.
