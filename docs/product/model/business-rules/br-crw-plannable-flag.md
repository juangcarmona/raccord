---
id: BR-CRW-PLANNABLE-FLAG
type: business-rule
title: "Only resources shown in planning can be planned"
status: draft
applies-to:
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
  - "UC-CRW-PLAN-RESOURCE-ON-SHIFT"
  - "UC-CRW-ADD-VEHICLE"
  - "UC-CRW-EDIT-CREW-MEMBERS"
uses-terms:
  - "TERM-CREW-MEMBER"
  - "TERM-VEHICLE"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014373260.md, .private/research/help-center/articles/14232209807506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A crew member or vehicle whose show-in-planning flag is off does not appear in planning views and cannot be planned on functions or shifts; likewise a function not marked plannable accepts no crew.

## Rationale

Keeps former staff, office-only staff and unused vehicles out of the way.

## Examples

A vehicle still owned but no longer used is set to not shown; it disappears from transport planning.

## Exceptions

None.
