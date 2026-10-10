---
id: BR-FIN-NEW-SIGNATURE-NEW-NUMBER
type: business-rule
title: "A new digital signature requires a new quote number"
status: draft
applies-to:
  - "UC-FIN-REGENERATE-QUOTE"
  - "UC-FIN-RESPOND-TO-ONLINE-QUOTE"
uses-terms:
  - "TERM-SIGNED-QUOTE"
  - "TERM-COMMERCIAL-DOCUMENT-NUMBER"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

If the customer must sign changed content digitally, a new quote with a new number is required; a new version or overwrite of a quote does not request a new signature.

## Rationale

A signature must refer to an unambiguous document.

## Examples

After signing, extra lights are added: a new quote number is generated for the customer to sign.

## Exceptions

None.
