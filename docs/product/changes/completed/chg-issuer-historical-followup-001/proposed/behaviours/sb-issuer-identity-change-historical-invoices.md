---
id: SB-ISSUER-IDENTITY-CHANGE-HISTORICAL-INVOICES
type: structured-behaviour
title: "Correct a historical invoice individually after an issuer identity change"
status: draft
illustrates:
  - "BR-PARTY-SNAPSHOT"
  - "UC-DEFINE-ISSUER-PROFILE"
  - "UC-CORRECT-ISSUED-INVOICE"
given:
  - "Invoice F2026-00001 was issued with the issuer identity recorded at that time"
  - "The same issuer's legal identity was later changed through the approved issuer-profile process"
when: "The billing administrator separately starts a rectifying invoice for F2026-00001 after determining that this specific invoice needs correction"
then:
  - "The rectifying invoice references F2026-00001 and follows the existing rectifying-invoice process"
  - "F2026-00001 retains its original issuer snapshot"
  - "Other previously issued invoices remain unchanged and are not automatically selected for correction"
  - "Future invoices use the accepted issuer profile"
uses-terms:
  - "TERM-ISSUED-INVOICE"
  - "TERM-RECTIFYING-INVOICE"
---

## Intent

An issuer-profile identity change affects future invoices; any historical correction is initiated for a specific invoice rather than inferred or generated from the profile change.

## Boundaries

This behaviour does not decide whether a real invoice or a particular jurisdiction requires correction. That determination remains outside the product rule and needs appropriate accounting/legal review.
