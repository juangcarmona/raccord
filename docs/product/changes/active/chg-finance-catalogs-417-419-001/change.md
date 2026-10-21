---
id: CHG-FINANCE-CATALOGS-417-419-001
type: product-change
title: Accept reusable invoicing catalogs required by contact defaults
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add: []
  modify:
    - UC-FIN-CONFIGURE-INVOICE-MOMENTS
    - FR-FIN-INVOICE-MOMENTS
    - TERM-INVOICE-MOMENT
    - TERM-INVOICE-MOMENT-TRIGGER
    - BR-FIN-MOMENT-TRIGGER-DATES
    - BR-FIN-MOMENT-PERCENTAGES
    - BR-FIN-MOMENT-DEFAULTS
    - UC-FIN-CONFIGURE-PAYMENT-TERMS
    - TERM-PAYMENT-TERM
    - TERM-PAYMENT-METHOD
    - BR-FIN-DUE-DATE-FROM-TERM
    - UC-FIN-CONFIGURE-VAT
    - FR-FIN-VAT
    - TERM-VAT-CLASS
    - TERM-VAT-SCHEME
    - TERM-VAT-SCHEME-KIND
    - BR-FIN-VAT-FROM-CLASS-AND-SCHEME
    - BR-FIN-USED-RATE-FROZEN
    - BR-FIN-DEFAULT-VAT-CLASS
    - UC-FIN-CONFIGURE-DISCOUNT-GROUPS
    - FR-FIN-101
    - FR-FIN-DISCOUNTS
    - TERM-DISCOUNT-GROUP
    - BR-FIN-DISCOUNT-GROUP-PERCENTAGE
  remove: []
---

## Problem

The reusable invoice-moment, payment-term, VAT-class/scheme and equipment discount-group catalogs required by #300 and #323 are described only in draft Product Definition artifacts. The application has no corresponding catalog models, and those draft definitions are not yet accepted product behavior.

## Intended Product Outcome

The Product Definition makes the existing configuration behavior for invoice moments, payment terms, VAT classes and schemes, and discount groups active and available to implementation. Contacts and projects can reference configured values by stable identity, while catalog configuration remains separate from copying a contact's values into a newly created project.

## Rationale

Contact defaults cannot be configured or copied safely when the values they reference have no catalog identity or lifecycle. Accepting these catalog capabilities first gives #300 and staged invoicing #323 stable product contracts without inventing local text substitutes.

## Affected Product Areas

Invoicing configuration, VAT configuration, equipment discount groups, customer commercial terms, contact defaults and project commercial settings.

## Open Questions

- Should the existing draft VAT classes be seeded by default for the Spanish installation, or must an administrator create all classes and schemes? Any seeded rates or tax treatment needs verified product and legal/tax evidence.
- Should contact discount defaults store a percentage for each discount-group identity shared by rental and sale equipment, or separate rental and sale percentage sets? The approved #300 Product Change names rental and sale discount groups but does not define this mapping.

## Product Acceptance

- The configuration use cases and their governing catalog terms and rules are active, internally consistent and validated in the Product Definition.
- Administrators can configure invoice moments, payment terms, VAT classes/schemes and equipment discount groups according to their accepted use cases.
- Invalid configurations are rejected without partial updates; used VAT rates remain historically stable.
- Contact and project defaults refer to configured catalog identities; changing a catalog does not rewrite values already copied to a project.
- This Product Change makes no claim of legal or tax compliance and does not define unverified VAT rates.

## Out of Scope

Staged invoice generation and queue processing beyond the catalog setup covered by #323, quote generation, payment execution, changing existing project values, tax-authority integration, legal advice, implementation, verification, release and deployment.
