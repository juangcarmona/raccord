---
id: BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK
type: business-rule
title: "Project cancellation preserves issued invoices and invoiced work"
status: active
applies-to:
  - "UC-PRJ-CHANGE-PROJECT-STATUS"
  - "UC-PRJ-CANCEL-PROJECT"
uses-terms:
  - "TERM-ISSUED-INVOICE"
provenance:
  source: "GitHub issue #303 acceptance criteria"
  confidence: high
  recovered-from: observation
---

## Rule

Changing a project's status or cancelling it never deletes, changes, reissues or removes the association to an issued invoice or the project work represented by that invoice. Corrections to an issued invoice use the product's correcting-invoice workflow.

## Rationale

Project lifecycle changes must preserve the commercial record and the work already invoiced.

## Examples

A project with an issued invoice is cancelled; the issued invoice and its work remain available in the project's history without changes.

## Exceptions

None.
