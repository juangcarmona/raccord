---
id: BR-WSP-EMPTY-BEFORE-PRODUCTION
type: business-rule
title: "Automatic emptying is only possible before production use"
status: draft
applies-to:
  - "UC-WSP-EMPTY-SAMPLE-DATA"
uses-terms:
  - "TERM-SAMPLE-DATA"
provenance:
  source: ".private/research/help-center/articles/360013982380.md, .private/research/help-center/articles/360015900179.md, .private/research/help-center/articles/360015603179.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

The installation can be emptied automatically only while it is still in evaluation, at the latest when it is put into production; afterwards records can only be archived or deleted by hand. Emptying cannot be undone.

## Rationale

Prevents wiping real business data.

## Examples

- A workspace in production use no longer offers automatic emptying.

## Exceptions

None.
