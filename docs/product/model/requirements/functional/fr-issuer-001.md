---
id: FR-ISSUER-001
type: functional-requirement
title: "Maintain the issuer profile"
status: draft
derived-from:
  - "UC-DEFINE-ISSUER-PROFILE"
  - "BR-PARTY-SNAPSHOT"
verification:
  - scenario: "An address or contact-detail update is applied to the next invoice, while an invoice issued before the update retains the previous details"
  - scenario: "After the first invoice is issued, a legal-name or tax-identifier change takes effect only after a second distinct authorized administrator confirms it with a reason"
  - scenario: "An issuer legal-identity change is audited and does not alter any previously issued invoice"
uses-terms:
  - "TERM-ISSUER"
---

## Requirement

The product MUST let an authorized administrator maintain the issuer's legal name, tax identifier, postal address and contact details, and MUST apply accepted profile changes only to invoices issued afterwards. Before any invoice has been issued, one authorized administrator may save a legal-identity change. After an invoice has been issued, a change to the legal name or tax identifier MUST include a reason and require confirmation by a second, distinct administrator with issuer-settings permission before it takes effect. The product MUST audit the request, decision, actors and reason. A change to a different issuing legal entity MUST use a separate installation.

## Rationale

Issuer details must be deliberate and traceable, while every issued invoice remains a reproducible snapshot of the issuer details used at issue time.
