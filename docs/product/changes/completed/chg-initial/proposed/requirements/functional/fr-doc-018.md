---
id: "FR-DOC-018"
type: functional-requirement
title: "E-mail documents with signature and tracked status"
status: draft
derived-from:
  - "UC-DOC-EMAIL-DOCUMENT"
  - "UC-DOC-MAINTAIN-EMAIL-SIGNATURE"
  - "BR-DOC-SIGNATURE-APPENDED"
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
verification:
  - scenario-ref: "SB-DOC-SIGNATURE-APPENDED"
  - scenario-ref: "SB-DOC-EMAIL-SENT-CONFIRMED"
  - scenario-ref: "SB-DOC-EMAIL-BOUNCED"
uses-terms:
  - "TERM-OUTGOING-EMAIL"
  - "TERM-EMAIL-DELIVERY-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/360014391919.md, .private/research/help-center/articles/360014259799.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST send generated documents by e-mail with editable recipients, subject and text, the sender's signature, and MUST track each e-mail's delivery status as pending, sent or delivery failed with the failure reason, through the company's own configured mail account.

## Rationale

Users must know whether documents arrived.
