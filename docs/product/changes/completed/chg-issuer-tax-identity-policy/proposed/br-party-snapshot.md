---
id: BR-PARTY-SNAPSHOT
type: business-rule
title: "Issued invoices keep the party details of their issue time"
status: draft
applies-to:
  - "UC-ISSUE-INVOICE"
  - "UC-DEFINE-ISSUER-PROFILE"
  - "UC-REGISTER-CUSTOMER"
uses-terms:
  - "TERM-ISSUER"
  - "TERM-CUSTOMER"
  - "TERM-ISSUED-INVOICE"
---

## Rule

Issuing freezes the issuer's and recipient's legal details on the invoice. Later changes to an issuer's address or contact details, or an approved change to the same issuer's legal identity, affect future invoices only. After an invoice has been issued, a legal-name or tax-identifier change requires a reason and confirmation by a second, distinct administrator with issuer-settings permission. A change to a different issuing legal entity requires a separate installation. Customer-profile changes also affect future invoices only.

## Rationale

An issued invoice must show the party details used when it was issued, and a change to issuer legal identity after issuing begins requires deliberate, traceable review.

## Examples

Changing the issuer's address after issuing F2026-00001 leaves F2026-00001 unchanged. A requested tax-identifier change after issuing begins remains pending until a second authorized administrator confirms it with a reason; after acceptance, only future invoices use it.

## Exceptions

None.
