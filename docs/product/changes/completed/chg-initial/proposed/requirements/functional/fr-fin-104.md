---
id: FR-FIN-104
type: functional-requirement
title: "Withdraw a published online quote"
status: draft
derived-from:
  - "UC-FIN-TAKE-ONLINE-QUOTE-OFFLINE"
verification:
  - id: "QUOTE-TAKEN-OFFLINE"
    scenario: "Given a published online quote, when the coordinator takes it offline, opening the quote link no longer shows the quote; when the coordinator later publishes the latest version again, the link works again."
uses-terms:
  - "TERM-ONLINE-QUOTE-STATUS"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a coordinator take a published online quote offline so that its link becomes unavailable to the customer, and MUST let the coordinator publish the latest version again afterwards.

## Rationale

An offer that is withdrawn or being revised must stop being visible to the customer.
