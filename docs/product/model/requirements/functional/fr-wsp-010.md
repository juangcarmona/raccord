---
id: FR-WSP-010
type: functional-requirement
title: "Configurable user roles"
status: draft
derived-from:
  - "UC-WSP-MANAGE-ROLE"
  - "BR-WSP-HIDDEN-MODULES"
verification:
  - scenario-ref: "SB-WSP-OWN-INVOICES-ONLY"
  - scenario-ref: "SB-WSP-CREW-PLANNER-GRANTS-PROJECTS"
uses-terms:
  - "TERM-USER-ROLE"
  - "TERM-ROLE-PERMISSION"
provenance:
  source: ".private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let authorized users create and edit roles that grant modules and set each permission's level and ownership scope, and provide four predefined, editable roles: Admin, Office, Warehouse and Crew.

## Rationale

Each group of users needs fitting access.
