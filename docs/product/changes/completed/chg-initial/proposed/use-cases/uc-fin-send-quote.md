---
id: UC-FIN-SEND-QUOTE
type: use-case
title: "Download or e-mail a quote"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CUSTOMER"
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-TERMS-ATTACHMENT"
uses-terms:
  - "TERM-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/360013938099.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The customer receives the offer as a file.

## Trigger

The quote is ready.

## Preconditions

A generated quote or contract exists.

## Main Flow

1. The coordinator downloads the document as PDF or word-processor file, or e-mails it from the product.
2. The product attaches the terms and conditions when the setting for this document type is on.

## Alternative Flows

The coordinator lets the customer sign digitally instead of returning a signed copy.

## Failure Conditions

Unsaved changes are not in the file until the document is regenerated.

## Postconditions

The customer has the offer.
