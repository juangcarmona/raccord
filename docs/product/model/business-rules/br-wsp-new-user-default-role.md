---
id: BR-WSP-NEW-USER-DEFAULT-ROLE
type: business-rule
title: "New users receive the Crew role"
status: draft
applies-to:
  - "UC-WSP-ASSIGN-ROLE"
uses-terms:
  - "TERM-PREDEFINED-ROLE"
provenance:
  source: ".private/research/help-center/articles/360012611160.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A newly added user gets the Crew role, which shows their own schedule and the projects they are planned on, until someone assigns another role.

## Rationale

Least privilege by default.

## Examples

- A crew member added without choosing a role can only consult their schedule.

## Exceptions

None.
