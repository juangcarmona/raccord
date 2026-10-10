---
id: SB-LOST-DOCUMENT-REGENERATED
type: structured-behaviour
title: "A lost invoice document is reproduced identically"
status: draft
illustrates:
  - "UC-OBTAIN-INVOICE-DOCUMENT"
given:
  - "An issued invoice's stored document has been lost"
when: "The billing administrator downloads the document"
then:
  - "The document is identical to the original"
  - "The regeneration appears in the invoice's audit trail"
uses-terms: []
---

## Intent

Documents are reproducible from the issued invoice.

## Boundaries

None.
