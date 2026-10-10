---
id: BR-FIN-SIGN-AFTER-EXPIRY
type: business-rule
title: "Signing after expiry depends on a company setting"
status: draft
applies-to:
  - "UC-FIN-RESPOND-TO-ONLINE-QUOTE"
  - "UC-FIN-CONFIGURE-FINANCIAL-SETTINGS"
uses-terms:
  - "TERM-EXPIRED-QUOTE"
  - "TERM-SIGNED-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

Customers can sign an expired quote or contract only when the company has enabled signing after the expiration date.

## Rationale

Some companies honour late acceptances, others require a fresh offer.

## Examples

With the setting off, signing a quote expired yesterday is refused.

## Exceptions

None.
