---
id: BR-AUDITED-ACTIONS
type: business-rule
title: "Sensitive actions are audited"
status: draft
applies-to:
  - "BC-INVOICING"
  - "BC-COMPLIANCE"
uses-terms:
  - "TERM-AUDIT-EVENT"
---

## Rule

Creating a draft, removing a draft line, discarding a draft, issuing an invoice, storing or regenerating its document and changing the issuer profile each record an audit event in the same operation; audit events are never changed or deleted.

## Rationale

Traceability of financial actions.

## Examples

Issuing records the number, issue date, total and fingerprint.

## Exceptions

None.
