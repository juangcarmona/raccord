---
id: BR-WHS-KIT-CONTENT-EDITABILITY
type: business-rule
title: "Kit content may be edited in a project only when allowed"
status: draft
applies-to:
  - "UC-WHS-ADJUST-KIT-CONTENT"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/19895194067730.md, .private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Content of a planned combination can be added, removed or re-quantified in a project only if the combination allows content editing in projects, which is possible only for virtual combinations; content marked as reserved from stock can never be removed or re-quantified in a project.

## Rationale

Fixed kits must stay intact; flexible kits must adapt to the job.

## Examples

See SB-WHS-RESERVED-FROM-STOCK-CONTENT-LOCKED.

## Exceptions

Physical combinations are never editable.
