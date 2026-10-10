---
id: "JRN-DOC-SEND-DOCUMENT-AND-CONFIRM-RECEIPT"
type: journey
title: "Send a document and confirm it arrived"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
steps:
  - use-case: "UC-FIN-GENERATE-QUOTE"
  - use-case: "UC-DOC-GENERATE-DOCUMENT"
  - use-case: "UC-DOC-APPLY-TEXT-TEMPLATE"
  - use-case: "UC-DOC-EMAIL-DOCUMENT"
  - use-case: "UC-DOC-RECORD-DELIVERY-OUTCOME"
  - use-case: "UC-DOC-REVIEW-RECIPIENT-COMMUNICATIONS"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/29923797058194.md, .private/research/help-center/articles/360014260839.md, .private/research/help-center/articles/21646575802258.md"
  confidence: medium
  recovered-from: documentation
---

## Intended Outcome

The customer has the document and the coordinator can prove when it was sent.

## Entry Conditions

A project with a quotation or invoice to send exists.

## Journey Narrative

The coordinator generates the document, starts the e-mail from a text template, sends it, and later checks the customer's communications, where the e-mail shows as sent with its date.

## Variants and Branches

Delivery failed: the coordinator investigates (UC-DOC-INVESTIGATE-DELIVERY-FAILURE) and resends. For invoices, UC-FIN-EMAIL-INVOICE applies; for online quotes, UC-FIN-PUBLISH-ONLINE-QUOTE.

## Completion Conditions

The e-mail is sent, or the failure cause is known.
