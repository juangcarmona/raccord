---
id: "BR-DOC-STATUS-EMAIL-ONLY"
type: business-rule
title: "Delivery status exists only for e-mails"
status: draft
applies-to:
  - "UC-DOC-REVIEW-RECIPIENT-COMMUNICATIONS"
  - "UC-DOC-REVIEW-COMMUNICATION-LOG"
uses-terms:
  - "TERM-COMMUNICATION-CHANNEL"
  - "TERM-EMAIL-DELIVERY-STATUS"
provenance:
  source: ".private/research/help-center/articles/29923797058194.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Only communication records on the e-mail channel show a delivery status; in-app notifications show none.

## Rationale

In-app notifications are delivered within the product.

## Examples

See SB-DOC-NOTIFICATION-HAS-NO-STATUS.

## Exceptions

None.
