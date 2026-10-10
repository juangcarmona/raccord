---
id: BR-CRW-IMPORT-EXCLUSIONS
type: business-rule
title: "Some crew data cannot be imported"
status: draft
applies-to:
  - "UC-CRW-IMPORT-CREW-MEMBERS"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
provenance:
  source: ".private/research/help-center/articles/360014366299.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Crew imports cannot set user roles, passwords or crew rates; every imported crew member receives the default user role.

## Rationale

These settings carry security or pricing consequences and are set individually.

## Examples

A file with a role column still yields crew members with the default role.

## Exceptions

None.
