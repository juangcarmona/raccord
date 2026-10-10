---
id: BR-WSP-HIDDEN-MODULES
type: business-rule
title: "Modules not granted are invisible"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-ROLE-PERMISSION"
provenance:
  source: ".private/research/help-center/articles/360012611160.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A module that a role does not grant is not shown to its users at all.

## Rationale

Reduces clutter and accidental exposure.

## Examples

- Freelancers without vehicle access do not see the vehicle module.

## Exceptions

- The communication module is always accessible; its sensitive content is permission-controlled.
