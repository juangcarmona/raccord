---
id: BR-CRW-SELF-PRESELECTED
type: business-rule
title: "The acting user is preselected in their own schedule"
status: draft
applies-to:
  - "UC-CRW-ADD-APPOINTMENT"
  - "UC-CRW-ENTER-CREW-AVAILABILITY"
  - "UC-CRW-DECLARE-OWN-AVAILABILITY"
uses-terms:
  - "TERM-APPOINTMENT"
  - "TERM-AVAILABILITY-ENTRY"
provenance:
  source: ".private/research/help-center/articles/20884029308306.md, .private/research/help-center/articles/360014685859.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When availability or an appointment is created from the personal schedule, the acting user is selected as a participant by default and must be deselected to act only for others.

## Rationale

Most entries concern oneself.

## Examples

A planner adding an appointment for two colleagues removes themselves first.

## Exceptions

None.
