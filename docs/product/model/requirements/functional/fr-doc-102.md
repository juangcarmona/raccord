---
id: FR-DOC-102
type: functional-requirement
title: "Explain why an e-mail was not delivered"
status: draft
derived-from:
  - "UC-DOC-INVESTIGATE-DELIVERY-FAILURE"
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
verification:
  - scenario-ref: "SB-DOC-EMAIL-ACCEPTED-BUT-SPAM"
uses-terms:
  - "TERM-EMAIL-DELIVERY-FAILED"
provenance:
  source: ".private/research/help-center/articles/21646575802258.md, .private/research/help-center/articles/360014199640.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST show, for every message in the sent list, its delivery status and, when delivery failed, the reason, distinguishing at least an invalid or blocked recipient address, rejection by the recipient's server and a problem of the company's mail account. The product MUST let the user correct the address or the mail account setup and retry, and MUST make clear that a message reported as sent may still have been filed as spam by the recipient.

## Rationale

A missing message erodes trust in the product, so users need to find and remove the cause themselves.
