---
id: BR-INT-SYNC-RESPECTS-ROLE
type: business-rule
title: "Calendar feeds show only what the role allows"
status: draft
applies-to:
  - "UC-INT-SUBSCRIBE-TO-CALENDAR-FEED"
uses-terms:
  - "TERM-CALENDAR-STREAM"
provenance:
  source: ".private/research/help-center/articles/360014047779.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Every stream delivers only items the user's role allows the user to see in the product.

## Rationale

Calendar feeds must not leak data.

## Examples

A user without access to all functions sees only their own functions.

## Exceptions

None.
