---
id: "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
type: business-rule
title: "E-mail delivery status follows the mail account's reports"
status: draft
applies-to:
  - "UC-DOC-RECORD-DELIVERY-OUTCOME"
  - "UC-DOC-REVIEW-SENT-EMAILS"
uses-terms:
  - "TERM-EMAIL-DELIVERY-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/29923797058194.md, .private/research/help-center/articles/21646575802258.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An outgoing e-mail starts as pending and becomes sent when the company's mail account confirms acceptance, or delivery failed when the mail account or the recipient's server refuses it; no other transitions exist.

## Rationale

The status must reflect what actually happened to the message.

## Examples

See SB-DOC-EMAIL-SENT-CONFIRMED and SB-DOC-EMAIL-BOUNCED.

## Exceptions

Messages accepted but filed as spam remain sent.
