---
id: BR-COMPLIANCE-OBLIGATION
type: business-rule
title: "Every issued invoice creates its compliance record"
status: draft
applies-to:
  - "UC-ISSUE-INVOICE"
  - "BC-COMPLIANCE"
uses-terms:
  - "TERM-COMPLIANCE-RECORD"
---

## Rule

Issuing an invoice and creating its compliance record happen together or not at all; compliance records are never deleted.

## Rationale

No invoice may escape tax reporting, including invoices issued before reporting is enabled.

## Examples

Issuing F2026-00001 creates one pending compliance record.

## Exceptions

None.
