---
id: BR-PRJ-SERVICE-EDIT-RIGHTS
type: business-rule
title: "Task rights do not grant service rights"
status: draft
applies-to:
  - "UC-PRJ-MANAGE-SERVICES"
uses-terms:
  - "TERM-PRODUCTION-SERVICE"
provenance:
  source: ".private/research/help-center/articles/38423504883858.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Being able to edit or register hours on a task does not allow editing the service it belongs to; services, including price, are edited only by users allowed to edit project finances.

## Rationale

Pricing is a financial decision.

## Examples

An assignee logs hours but cannot change the service price.

## Exceptions

None.
