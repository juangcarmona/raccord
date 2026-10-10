---
id: BR-SINGLE-BILLING
type: business-rule
title: "A billable line is billed once"
status: draft
applies-to:
  - "UC-CREATE-INVOICE-DRAFT"
  - "UC-REVISE-INVOICE-DRAFT"
  - "UC-ISSUE-INVOICE"
uses-terms:
  - "TERM-BILLING-STATE"
---

## Rule

Only an unbilled line can be added to a draft; a line belongs to at most one open draft; removing it from the draft or discarding the draft makes it unbilled again; issuing makes it invoiced for ever.

## Rationale

Double billing and forgotten work are the costliest billing errors.

## Examples

See SB-LINE-IN-TWO-DRAFTS.

## Exceptions

None.
