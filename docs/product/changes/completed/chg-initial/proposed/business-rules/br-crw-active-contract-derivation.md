---
id: BR-CRW-ACTIVE-CONTRACT-DERIVATION
type: business-rule
title: "A crew member has an active contract when a contract document covers today"
status: draft
applies-to:
  - "UC-CRW-MANAGE-CREW-DOCUMENTS"
uses-terms:
  - "TERM-ACTIVE-CONTRACT"
provenance:
  source: ".private/research/help-center/articles/26200585082258.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A crew member's active contract is derived by default from a contract-type document with a start date on or before today and an end date on or after today (or none); its end date is shown as the current contract end. An administrator may override the derived active-contract result when the company's records require it.

## Rationale

Planners check contractual cover before assigning work.

## Examples

Contract from 1 Jan to 30 Jun: active on 15 Mar, end date 30 Jun.

An administrator may override the derived result when the uploaded documents do not reflect the company's current contractual decision.

## Exceptions

A contract document without an end date counts as active from its start date onwards.
