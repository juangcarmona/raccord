---
id: BR-WSP-DASHBOARD-RESPECTS-PERMISSIONS
type: business-rule
title: "Dashboards only show permitted information"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-DASHBOARD-WIDGET"
  - "TERM-ROLE-PERMISSION"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A dashboard only shows information the user may see in the corresponding modules; without financial visibility no financial widgets can be added.

## Rationale

The dashboard must not be a side door.

## Examples

- A user without financial visibility cannot add a revenue widget.

## Exceptions

None.
