---
id: BR-WSP-TAGS-WITHIN-ACCESSIBLE-MODULES
type: business-rule
title: "Tag management is limited to accessible modules"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Users allowed to create and delete tags can do so only in modules they can access.

## Rationale

Prevents changes to classification of hidden records.

## Examples

- A warehouse worker without crew member access cannot create crew tags.

## Exceptions

None.
