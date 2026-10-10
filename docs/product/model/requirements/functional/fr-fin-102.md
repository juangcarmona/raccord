---
id: FR-FIN-102
type: functional-requirement
title: "Deliver a quote to the customer as a file"
status: draft
derived-from:
  - "UC-FIN-SEND-QUOTE"
  - "BR-FIN-TERMS-ATTACHMENT"
verification:
  - id: "QUOTE-DELIVERY-WITH-TERMS"
    scenario: "Given a generated quote and the setting to attach terms for quotes switched on, when the coordinator downloads it or e-mails it from the product, the customer receives the quote with the terms and conditions attached; changes made after generation are absent until the document is regenerated."
uses-terms:
  - "TERM-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/360013938099.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a coordinator download a generated quote or contract as a PDF or word-processor file or e-mail it from the product, and MUST attach the terms and conditions when the setting for that document type is on. The product MUST NOT include unsaved or later changes in the file until the document is regenerated, and MUST let the customer sign digitally as an alternative to returning a signed copy.

## Rationale

The customer needs the offer in a form they can keep, review and sign.
