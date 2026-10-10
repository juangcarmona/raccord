---
id: BR-PRJ-EQUIPMENT-IMPORT-MODES
type: business-rule
title: "Equipment list import appends or replaces"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-EQUIPMENT-LIST"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/4413562661138.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Importing an equipment list into a project either adds all rows as new lines, or deletes all planned equipment and adds all rows; existing planned lines can never be partially overwritten.

## Rationale

Keeps import outcomes predictable.

## Examples

Re-importing a revised drawing in replace mode removes previously planned items first.

## Exceptions

None.
