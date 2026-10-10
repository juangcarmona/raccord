---
id: "UC-DOC-INVESTIGATE-DELIVERY-FAILURE"
type: use-case
title: "Investigate an undelivered e-mail"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
uses-terms:
  - "TERM-EMAIL-DELIVERY-FAILED"
provenance:
  source: ".private/research/help-center/articles/21646575802258.md, .private/research/help-center/articles/360014199640.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

The cause of a missing e-mail is understood and addressed.

## Trigger

A recipient reports not receiving a message, or a message shows delivery failed.

## Preconditions

The e-mail is in the sent e-mails list.

## Main Flow

1. The user opens the message and reads its status and failure reason.
2. The user identifies the cause: wrong or blocked recipient address, recipient server or spam rejection, or a problem with the company's mail account.
3. The user corrects the address, asks the recipient to allow the sender, checks the mail account configuration or retries later.

## Alternative Flows

Status is sent but the recipient cannot find it: the message was probably filed as spam by the recipient.

## Failure Conditions

Outages of the company's mail provider cannot be resolved in the product.

## Postconditions

The cause is known and remedial action taken.
