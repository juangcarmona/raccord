---
id: BR-ISSUE-DATE-ORDER
type: business-rule
title: "Issue dates follow number order within a series"
status: draft
applies-to:
  - "UC-ISSUE-INVOICE"
uses-terms:
  - "TERM-ISSUE-DATE"
---

## Rule

An invoice cannot be issued with an issue date earlier than the last invoice of its series in that year; the same date is allowed.

## Rationale

Numbers and dates must tell the same story to an inspector.

## Examples

See SB-BACKDATED-ISSUE-REFUSED.

## Exceptions

None.
