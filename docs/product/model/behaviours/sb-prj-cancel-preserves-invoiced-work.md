---
id: SB-PRJ-CANCEL-PRESERVES-INVOICED-WORK
type: structured-behaviour
title: "Cancellation preserves issued invoices and invoiced work"
status: active
illustrates:
  - "BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK"
  - "UC-PRJ-CANCEL-PROJECT"
given:
  - "A project has an issued invoice and remaining open work"
when: "The coordinator cancels the project and confirms applicable cancellation actions"
then:
  - "The issued invoice remains unchanged and available in the project's history"
  - "The invoiced work represented by that invoice remains associated with the project"
  - "Only the open work affected by the confirmed cancellation actions is changed"
uses-terms:
  - "TERM-ISSUED-INVOICE"
provenance:
  source: "GitHub issue #303 acceptance criteria"
  confidence: high
  recovered-from: observation
---

## Intent

Cancelling a project does not erase or rewrite work that has already been invoiced.

## Boundaries

Corrections to an issued invoice follow the separate correcting-invoice workflow.
