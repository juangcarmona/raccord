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

Creating a draft, removing a draft line, discarding a draft, issuing an invoice, storing or regenerating its document and changing the issuer profile each record an audit event in the same operation; audit events are never changed or deleted. A legal-identity change requested after invoice issuance records the request, reason, old and proposed values, initiator, reviewer, decision and time. The initiator and reviewer must be distinct administrators. Rejected and abandoned requests remain auditable.

## Rationale

Traceability of financial actions and review of issuer legal identity preserve accountability when a profile update can change future invoice identity.

## Examples

Issuing records the number, issue date, total and fingerprint. Confirming a post-issuance tax-identifier change records the reason, both administrators and the decision.

## Exceptions

None.
