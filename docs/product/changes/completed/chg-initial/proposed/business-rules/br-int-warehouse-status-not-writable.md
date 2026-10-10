---
id: BR-INT-WAREHOUSE-STATUS-NOT-WRITABLE
type: business-rule
title: "Warehouse statuses are not written to project status"
status: draft
applies-to:
  - "UC-INT-WRITE-RECORDS-THROUGH-API"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/37629880908050.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Since project-management and warehouse statuses are separate, writing a warehouse status (such as prepped or on location) into the status of a project is refused; project statuses and warehouse statuses are read from separate resources.

## Rationale

Warehouse progress is tracked per packing moment, not on the project.

## Examples

Setting a project to on location through the interface is refused.

## Exceptions

None.
