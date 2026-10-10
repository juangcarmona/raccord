---
id: BR-WHS-READY-TO-PACK-LOCK
type: business-rule
title: "Ready to pack cannot be withdrawn after packing started"
status: draft
applies-to:
  - "UC-WHS-MARK-READY-TO-PACK"
uses-terms:
  - "TERM-READY-TO-PACK"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Once any equipment of a project has been booked in the warehouse, its ready-to-pack flag can no longer be switched off.

## Rationale

Equipment has already physically moved.

## Examples

A planner trying to reopen planning after the first case was packed is refused.

## Exceptions

None.
