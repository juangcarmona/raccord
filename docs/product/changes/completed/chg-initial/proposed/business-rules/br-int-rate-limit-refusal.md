---
id: BR-INT-RATE-LIMIT-REFUSAL
type: business-rule
title: "Requests beyond the rate limit are refused"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
  - "UC-INT-WRITE-RECORDS-THROUGH-API"
uses-terms:
  - "TERM-API-RATE-LIMIT"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

Requests above the allowed rate are refused with a forbidden-type response on any endpoint, whether or not its description lists that response.

## Rationale

Protects the service from runaway integrations.

## Examples

A tight polling loop starts receiving refusals.

## Exceptions

None.
