---
id: BR-FIN-EXTERNAL-INVOICING
type: business-rule
title: "Marking as invoiced externally removes the reminder"
status: draft
applies-to:
  - "UC-FIN-MARK-INVOICED-EXTERNALLY"
uses-terms:
  - "TERM-EXTERNALLY-INVOICED"
provenance:
  source: ".private/research/help-center/articles/25948227210258.md, .private/research/help-center/articles/360013628279.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A project marked as invoiced externally leaves the invoicing queue and is listed with externally invoiced items.

## Rationale

Some invoices must come from other certified software.

## Examples

A project billed with other software is marked and no longer appears as waiting to be invoiced.

## Exceptions

None.
