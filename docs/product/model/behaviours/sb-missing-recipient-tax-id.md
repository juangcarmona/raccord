---
id: SB-MISSING-RECIPIENT-TAX-ID
type: structured-behaviour
title: "Issuing is refused when the recipient lacks a tax identifier"
status: draft
illustrates:
  - "BR-ISSUE-READINESS"
given:
  - "A draft's customer has no tax identifier and no address"
when: "The billing administrator issues the draft"
then:
  - "Nothing is issued"
  - "The refusal lists the missing recipient tax identifier and the incomplete recipient address"
  - "No invoice number is consumed"
uses-terms: []
---

## Intent

Every blocking reason is reported at once.

## Boundaries

None.
