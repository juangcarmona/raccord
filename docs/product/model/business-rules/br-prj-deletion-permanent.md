---
id: BR-PRJ-DELETION-PERMANENT
type: business-rule
title: "Deleting a project is permanent"
status: draft
applies-to:
  - "UC-PRJ-DELETE-PROJECT"
uses-terms:
  - "TERM-PROJECT"
provenance:
  source: ".private/research/help-center/articles/360013367659.md, .private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A deleted project cannot be recovered. Permanent deletion is allowed only when the project has no issued invoices, audit events, compliance records, payments or legal workday data; otherwise the project must be cancelled or archived. Cancelling is preferred whenever history should be kept.

## Rationale

Deletion removes data from statistics and history, so legally relevant or auditable history must remain available.

## Examples

A deleted project no longer appears in statistics; a cancelled one does.

## Exceptions

Projects with legally relevant or auditable records cannot be permanently deleted.
