---
id: BR-WSP-WAREHOUSE-ALL-PROJECTS
type: business-rule
title: "Seeing all projects in the warehouse requires all projects in projects"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-ROLE-PERMISSION"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The warehouse shows all projects only when the role may see all projects in the projects area; otherwise it is limited to projects the user is planned on or assigned to.

## Rationale

The warehouse must not reveal projects hidden elsewhere.

## Examples

- A warehouse worker with own-projects-only visibility sees just the jobs they are assigned to in the warehouse.

## Exceptions

None.
