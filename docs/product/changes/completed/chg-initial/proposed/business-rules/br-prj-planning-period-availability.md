---
id: BR-PRJ-PLANNING-PERIOD-AVAILABILITY
type: business-rule
title: "Planned resources are unavailable during the planning period"
status: draft
applies-to:
  - "BC-PROJECTS"
uses-terms:
  - "TERM-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360017700339.md, .private/research/help-center/articles/360013599680.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment and crew planned on a project are counted as unavailable to other projects for the whole planning period linked to them.

## Rationale

The equipment physically leaves the warehouse for that time.

## Examples

A mixer planned with planning period 1 to 5 May cannot be planned on another project on 3 May without a shortage.

## Exceptions

Cancelled projects (see BR-PRJ-CANCEL-RELEASES-EQUIPMENT).
