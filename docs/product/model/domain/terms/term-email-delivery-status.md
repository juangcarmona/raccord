---
id: "TERM-EMAIL-DELIVERY-STATUS"
type: domain-term
title: "E-mail delivery status"
status: draft
defined-in: "BC-DOCUMENTS"
synonyms: []
uses-terms:
  - "TERM-EMAIL-PENDING"
  - "TERM-EMAIL-SENT"
  - "TERM-EMAIL-DELIVERY-FAILED"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/29923797058194.md, .private/research/help-center/articles/21646575802258.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The lifecycle of an outgoing e-mail's delivery, shown for e-mails only.

| State | Meaning |
| --- | --- |
| Pending (TERM-EMAIL-PENDING) | The message was handed to the company's mail account; no answer yet. |
| Sent (TERM-EMAIL-SENT) | The mail account confirmed it accepted the message. |
| Delivery failed (TERM-EMAIL-DELIVERY-FAILED) | The mail account or the recipient's server refused the message (bounce); the reason is reported. |

Transitions: pending -> sent (acceptance confirmed); pending -> delivery failed (rejection reported). Sent and delivery failed are final for that message; delivery may still be slow while pending (deferral, soft failure). Acceptance does not guarantee the message reached the inbox: the recipient may still file it as spam.

## Distinguish From

Communication channel: in-app notifications have no delivery status. Quote status: a business state of the document, not of the e-mail.

## Usage

Shown in the sent e-mails list and per contact or crew member.
