---
id: BR-CRW-DEFAULT-BREAK
type: business-rule
title: "Registrations start with the default break"
status: draft
applies-to:
  - "UC-CRW-REGISTER-HOURS-MANUALLY"
  - "UC-CRW-REGISTER-HOURS-FROM-PLANNED-ACTIVITIES"
uses-terms:
  - "TERM-TIME-REGISTRATION"
provenance:
  source: ".private/research/help-center/articles/360015498839.md, .private/research/help-center/articles/360015498439.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

A new time registration starts with the break of the linked function or shift when it has one, otherwise with the company default break; the user may change it.

## Rationale

Saves typing; matches planning.

## Examples

Default 30 minutes; a shift with 45 minutes break prefills 45.

## Exceptions

None.
