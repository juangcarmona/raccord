---
id: BR-PRJ-WAREHOUSE-STATUS-PROMPT
type: business-rule
title: "Warehouse handling drives packing-list statuses, not project statuses"
status: draft
applies-to:
  - "UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY"
uses-terms:
  - "TERM-PROJECT-STATUS"
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Warehouse handling changes the warehouse status of packing lists, never the project status directly: when all equipment of a packing list reaches a stage the product proposes that stage as the packing list's status; the project status moves only through its own automatic transitions (In progress at the first booking, Execution done when everything is back) or by a user.

## Rationale

Equipment leaves and returns in several moments per project; one project status cannot describe it, and nothing may be marked done while equipment is still out.

## Examples

See SB-PRJ-ALL-ITEMS-MOVED-PROMPT, SB-PRJ-EARLY-RETURN-PARTIAL and SB-PRJ-ITEM-LEFT-BEHIND-NO-PROMPT.

## Exceptions

Users may still set any project status manually.
