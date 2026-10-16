---
id: CHG-ISSUER-TAX-IDENTITY-001
type: product-change
title: Protect issuer legal-identity changes after invoice issuance
status: applied
base-revision: "f2ce9597d42dafce2aae3e47dff24df7301673f5"
operations:
    add: []
    modify:
        - FR-ISSUER-001
        - UC-DEFINE-ISSUER-PROFILE
        - BR-PARTY-SNAPSHOT
        - BR-AUDITED-ACTIONS
        - CON-SINGLE-ISSUER
    remove: []
---

## Problem

The product definition allows the issuer profile's legal name, tax identifier and address to be changed through the same update flow. It preserves issued-invoice snapshots, but does not distinguish ordinary address or contact corrections from a change to the issuer's legal identity or require additional review after invoicing has begun.

## Intended Product Outcome

Address and contact-detail changes remain available to an authorized administrator and affect future invoices only. Once an invoice has been issued, changing the issuer's legal name or tax identifier requires a reason and confirmation by a second, distinct administrator with issuer-settings permission. The review decision and resulting profile change are audited. Issued invoices and their snapshots remain unchanged.

The installation continues to represent one issuing legal entity. A change that represents a different legal entity requires a separate installation; a correction or change to the same legal entity does not automatically require one.

## Rationale

The issuer's identity is a central invoice detail, so a mistaken update can affect future billing while historical invoices must remain reproducible. A second-person review creates the requested deliberate check and an audit trail without treating a new installation as a legal requirement for every tax-identifier correction.

The Spanish invoicing regulation requires issuer identification and tax-identification data on invoices and provides for rectifying invoices when an original lacks required particulars. Those provisions do not establish that a changed tax identifier always requires a new installation. This is a product safeguard, not a legal-compliance conclusion. Source reviewed 2026-10-16: [Real Decreto 1619/2012, especially Articles 6 and 15](https://www.boe.es/eli/es/rd/2012/11/30/1619/).

## Affected Product Areas

- Issuer-profile maintenance and future invoice content.
- Issued-invoice party snapshots and their audit history.
- The single-issuer-per-installation constraint.

## Open Questions

None. The policy decision is to require two-person review for legal-identity changes after the first issued invoice, to preserve every issued snapshot, and to use a separate installation only when the issuing legal entity is different. This decision does not determine whether any particular historical invoice requires legal correction.

## Product Acceptance

- The definition distinguishes address/contact changes from legal-identity changes.
- Legal-identity changes after the first issued invoice require a reason and approval by a second distinct authorized administrator.
- Accepted and rejected review decisions, including the reason and actors, are audited.
- Historical issued invoices remain unchanged; updates affect future invoices only.
- A different issuing legal entity uses a separate installation, while a change to the same entity does not automatically require one.
- The accepted definition makes no legal-compliance claim and leaves correction of specific invoices to an appropriately reviewed process.

## Out of Scope

This change does not implement the review flow, change any issuer identity, migrate an installation or modify an issued invoice. It does not decide whether a specific invoice must be rectified or assert legal or tax compliance. Those questions require qualified review before implementation for the jurisdictions supported.
