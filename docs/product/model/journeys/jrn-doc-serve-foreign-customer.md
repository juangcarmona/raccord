---
id: "JRN-DOC-SERVE-FOREIGN-CUSTOMER"
type: journey
title: "Serve a customer abroad"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
steps:
  - use-case: "UC-DOC-PREPARE-FOREIGN-LANGUAGE-TEMPLATE"
  - use-case: "UC-DOC-APPLY-TEXT-TEMPLATE"
  - use-case: "UC-DOC-EMAIL-DOCUMENT"
provenance:
  source: ".private/research/help-center/articles/360014194100.md, .private/research/help-center/articles/360013979460.md, .private/research/help-center/articles/360014260839.md"
  confidence: medium
  recovered-from: documentation
---

## Intended Outcome

A foreign customer receives documents in its language.

## Entry Conditions

Translations exist as custom fields.

## Journey Narrative

The documents are set to the customer's language and country, the quotation is generated, and it is e-mailed with a text template in the customer's language.

## Variants and Branches

None.

## Completion Conditions

The customer has a document in its language.
