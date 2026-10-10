---
id: JRN-FIN-QUOTE-TO-SIGNATURE
type: journey
title: "From quote to signed offer"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
steps:
  - use-case: "UC-FIN-REVIEW-PROJECT-FINANCIALS"
  - use-case: "UC-FIN-APPLY-CUSTOMER-DEFAULTS"
  - use-case: "UC-FIN-APPLY-PROJECT-DISCOUNTS"
  - use-case: "UC-FIN-GENERATE-QUOTE"
  - use-case: "UC-FIN-PUBLISH-ONLINE-QUOTE"
  - use-case: "UC-FIN-RESPOND-TO-ONLINE-QUOTE"
  - use-case: "UC-FIN-RESOLVE-CHANGE-REQUEST"
  - use-case: "UC-FIN-REGENERATE-QUOTE"
  - use-case: "UC-FIN-LOCK-PROJECT-PRICE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/29481848461458.md, .private/research/help-center/articles/360013264720.md"
  confidence: medium
  recovered-from: documentation
---

## Intended Outcome

A planned job is priced, offered and accepted by the customer at a stable price.

## Entry Conditions

A project with planned content and a client exists.

## Journey Narrative

The coordinator checks costs and margins, applies the client's conditions and negotiated discounts, generates a quote and publishes it online. The customer reviews it, requests changes; the coordinator resolves them by regenerating the quote, and the customer signs. The coordinator locks the price.

## Variants and Branches

PDF instead of online page; contract instead of quote; a new quote number when a fresh signature is required; quote expires before signing.

## Completion Conditions

A signed quote exists and the project price is locked.
