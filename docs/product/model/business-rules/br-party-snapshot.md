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

Issuing freezes the issuer's and the recipient's legal details on the invoice; later changes to the issuer profile or the customer affect only invoices issued afterwards.

## Rationale

An issued invoice must show what was true when it was issued.

## Examples

Renaming the issuer after issuing F2026-00001 leaves F2026-00001 unchanged.

## Exceptions

None.
