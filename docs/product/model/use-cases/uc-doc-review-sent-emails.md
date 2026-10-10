---
id: "UC-DOC-REVIEW-SENT-EMAILS"
type: use-case
title: "Review sent e-mails and their status"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-COMMUNICATION-ACCESS"
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
uses-terms:
  - "TERM-OUTGOING-EMAIL"
  - "TERM-EMAIL-DELIVERY-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/29923797058194.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user sees e-mails sent from the company's mail account, their content and delivery status.

## Trigger

A customer says a document did not arrive.

## Preconditions

The user's role permits viewing the e-mails.

## Main Flow

1. The user opens the sent e-mails list.
2. The user narrows it to a time period.
3. The user opens a message to read its content and status.

## Alternative Flows

None.

## Failure Conditions

Without the needed permissions, only own messages (or none) are shown.

## Postconditions

Nothing changes.
