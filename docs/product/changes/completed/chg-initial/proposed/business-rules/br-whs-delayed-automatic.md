---
id: BR-WHS-DELAYED-AUTOMATIC
type: business-rule
title: "Delayed is derived, never set"
status: draft
applies-to:
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
  - "UC-WHS-MARK-DELAYED"
  - "BC-WAREHOUSE"
uses-terms:
  - "TERM-DELAYED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/15239125718546.md, .private/research/help-center/articles/15125682136850.md, .private/research/help-center/articles/23280754011154.md, .private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A packing list is shown as delayed automatically when any of its equipment is still out after the end of its planning period; no one can set delayed manually, and it clears when everything is returned or a new expected return date is set.

## Rationale

Delays must reflect facts, not opinions.

## Examples

See SB-WHS-UNRETURNED-BECOMES-DELAYED.

## Exceptions

None.
