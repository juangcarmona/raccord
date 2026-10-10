---
id: BR-EQP-TYPE-FIXED-ONCE-PLANNED
type: business-rule
title: "Equipment type is fixed once planned"
status: draft
applies-to:
  - "UC-EQP-CHANGE-ITEM-TYPE"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013718240.md, .private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The type of an equipment item can be changed only while the item has never been planned on any project or project template; afterwards the type is immutable and a different type requires a new item.

## Rationale

Changing the type of planned equipment would alter existing projects, templates and documents: for example turning a virtual kit into a single item would strip its content from every project using it, and would distort availability, packing slips and statistics.

## Examples

- A virtual kit never planned can become a physical kit directly.
- A virtual kit planned last year must be duplicated as a physical kit and the original archived.

## Exceptions

None.
