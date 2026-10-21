---
id: CHG-FINANCE-CATALOGS-417-419-001
type: product-change
title: Accept reusable invoicing catalogs and seed the Spanish standard VAT default
status: applied
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

## Accepted Product Decisions

- Seed a Spanish standard-rate VAT class and default scheme with 21% as the initial value. The Agencia Tributaria lists 21% as the general rate for 2026 and says the rate applicable to an operation is the one in force at its tax point. This value is an editable setup default, not a determination that every item or service is subject to that rate. Verify the current official rate before creating a new installation's initial configuration; retain source and verification date with the default. Verified 2026-10-21 against [Agencia Tributaria, VAT rate](https://sede.agenciatributaria.gob.es/Sede/ayuda/manuales-videos-folletos/manuales-practicos/manual-iva-2026/capitulo-04-sujetos-pasivos-repercusion-impositivo/tipo-impositivo.html) and [Law 37/1992, article 90](https://www.boe.es/buscar/act.php?id=BOE-A-1992-28740).
- Create reduced, zero-rate, reverse-charge and other VAT classes or schemes only when configured by an administrator; do not pre-fill rates for those cases in this change.
- Contact discount defaults store one percentage per discount-group identity, shared across rental and sale equipment. No discount percentage is pre-filled.

## Open Questions

None. The standard rate and discount-group mapping are resolved as recorded above.

## Product Acceptance

- The configuration use cases and their governing catalog terms and rules are active, internally consistent and validated in the Product Definition.
- Administrators can configure invoice moments, payment terms, VAT classes/schemes and equipment discount groups according to their accepted use cases.
- A new Spanish installation starts with the standard VAT class and scheme using 21% as an editable initial default, with its official source and verification date recorded. The default does not decide the rate for a particular operation; administrators classify items and select or configure the applicable scheme.
- Invalid configurations are rejected without partial updates; used VAT rates remain historically stable. When the legal standard rate changes, a new scheme becomes the default and existing schemes remain unchanged.
- Contact and project defaults refer to configured catalog identities; changing a catalog does not rewrite values already copied to a project.
- This Product Change makes no claim that a particular operation is legally classified correctly or that the product is tax compliant. The 21% initial default is a source-dated setup value, not legal advice or a substitute for classifying each operation.

## Out of Scope

Staged invoice generation and queue processing beyond the catalog setup covered by #323, quote generation, payment execution, changing existing project values, tax-authority integration, legal advice, automatic monitoring of legal rates, implementation, verification, release and deployment.
