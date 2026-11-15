---
id: "UC-DOC-RECORD-DELIVERY-OUTCOME"
type: use-case
title: "Record an e-mail's delivery outcome"
status: draft
primary-actor: "ACT-EMAIL-DELIVERY-SERVICE"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
uses-terms:
  - "TERM-EMAIL-DELIVERY-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/29923797058194.md, .private/research/help-center/articles/21646575802258.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

Each e-mail shows whether the company's mail account sent it.

## Trigger

The mail account reports on a message.

## Preconditions

The e-mail is pending.

## Main Flow

1. The mail account reports acceptance of the message.
2. The product marks the e-mail sent.

## Alternative Flows

The mail account reports a rejection: the product marks the e-mail delivery failed with the reason.

## Failure Conditions

Outage or limits of the mail account delay or fail sending; the product cannot influence it.

## Postconditions

The status is final.
