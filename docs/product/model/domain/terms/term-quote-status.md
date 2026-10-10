---
id: TERM-QUOTE-STATUS
type: domain-term
title: "Quote status"
status: draft
defined-in: "BC-COMMERCIAL"
synonyms: []
uses-terms:
  - "TERM-QUOTE-VERSION"
  - "TERM-EXPIRED-QUOTE"
  - "TERM-SIGNED-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/29481848461458.md, .private/research/help-center/articles/360013938099.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

Lifecycle of a quote or contract version.

| State | Meaning |
| --- | --- |
| Current | The latest generated version, valid until its expiration date. |
| Superseded | An older version kept for reference after a new version was created. |
| Expired | The expiration date has passed without signature. |
| Signed | The customer signed the version digitally. |

Transitions: current -> superseded (new version); current -> expired (expiration date passes); current -> signed (customer signs); expired -> signed only when signing after expiry is allowed; overwriting keeps the state of the current version. A signed version can only be followed by a new quote number, not a new version, if the customer must sign again.

## Distinguish From

Online quote status, which tracks publication rather than validity.

## Usage

Projects can be filtered on having expired quotes.
