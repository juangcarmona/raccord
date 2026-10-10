---
id: BR-WSP-SERIES-CURRENT-NUMBER
type: business-rule
title: "A series' current number is the latest used"
status: draft
applies-to:
  - "UC-WSP-CONFIGURE-NUMBER-SERIES"
uses-terms:
  - "TERM-NUMBER-SERIES"
provenance:
  source: ".private/research/help-center/articles/15426355918226.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

The current number displayed for a number series is the number of the latest item created in that series; the next item continues from it.

## Rationale

Gives administrators a clear starting point when aligning with previous systems.

## Examples

- Setting the project series current number to 2026-099 makes the next project 2026-100.

## Exceptions

None.
