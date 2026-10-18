---
id: CHG-ISSUER-HISTORICAL-FOLLOWUP-001
type: product-change
title: Define case-by-case follow-up for invoices after issuer identity changes
status: applied
base-revision: "38797490b2bdeacbfdae6bf7de56b50033c915b7"
operations:
    add:
        - SB-ISSUER-IDENTITY-CHANGE-HISTORICAL-INVOICES
    modify:
        - BR-PARTY-SNAPSHOT
    remove: []
---

## Problem

The Product Definition preserves the issuer details on an issued invoice and provides a rectifying-invoice flow, but does not state whether an accepted issuer-profile identity change automatically initiates follow-up for previously issued invoices.

## Intended Product Outcome

An accepted issuer-profile change applies only to future invoices. It does not alter, flag, queue or automatically create rectifying invoices for previously issued invoices. Historical invoices are considered individually; when a correction is separately determined to be needed, the existing rectifying-invoice flow applies to the selected invoice or invoices.

## Rationale

The issuer-profile change alone does not establish that a particular issued invoice is wrong or requires correction. Keeping the decision case-specific preserves immutable invoice history and avoids implying a blanket legal or accounting conclusion.

The Owner selected the product behaviour: profile changes do not trigger automatic historical corrections, and any correction is handled individually through the existing flow. No jurisdiction-specific accounting/legal answer is available. This change deliberately leaves that determination outside the product and does not make it a prerequisite for defining this product behaviour.

## Affected Product Areas

- Issuer-profile changes and the details used on future invoices.
- Preservation of issued-invoice snapshots.
- Case-specific correction of issued invoices.

## Open Questions

None. Whether a specific historical invoice requires correction is outside this Product Change; the product neither determines nor gives advice on that question.

## Product Acceptance

- The business rule states that accepted issuer-profile changes affect future invoices only and do not automatically initiate historical-invoice corrections.
- A structured behaviour shows one historical invoice being corrected through the existing rectifying-invoice flow while other historical invoices remain unaffected.
- The proposed definition does not determine which real invoices require correction or claim legal/accounting compliance.
- The absence of jurisdiction-specific accounting/legal advice does not change the defined product response and is not represented as settled legal guidance.

## Out of Scope

This change does not alter or delete issued invoices, determine legal treatment for any invoice or jurisdiction, advise operators which invoices require correction, implement a historical-invoice review queue, change the rectifying-invoice flow, or make a legal or tax compliance claim. Operators seek appropriate advice for individual circumstances outside the product.
