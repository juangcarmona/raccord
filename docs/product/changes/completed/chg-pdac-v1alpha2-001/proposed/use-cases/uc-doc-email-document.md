---
id: "UC-DOC-EMAIL-DOCUMENT"
type: use-case
title: "E-mail a generated document"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-EMAIL-DELIVERY-SERVICE"
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-SIGNATURE-APPENDED"
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
  - "BR-FIN-TERMS-ATTACHMENT"
uses-terms:
  - "TERM-OUTGOING-EMAIL"
  - "TERM-SENDER-ADDRESS"
  - "TERM-COMMUNICATION-RECORD"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/360014391919.md, .private/research/help-center/articles/360014260839.md, .private/research/help-center/articles/29923797058194.md, .private/research/help-center/articles/21646575802258.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

A recipient receives the document by e-mail and the sending is traceable.

## Trigger

A generated document must reach a customer, supplier or crew member.

## Preconditions

A document has been generated.

## Main Flow

1. The user chooses to e-mail the document.
2. The product pre-fills recipients, the subject from the template and the sender address of the company's mail account.
3. The user optionally applies an e-mail template and edits the text.
4. The user sends.
5. The product appends the user's signature, attaches the document, sends the message through the company's configured mail account and logs it as pending.

## Alternative Flows

For an online quote the message carries link and password instead of a PDF (commercial context).

## Failure Conditions

The company's mail account is not configured or refuses the message: the e-mail is logged as failed with the reason and the user can correct the cause and send again.

## Postconditions

A communication record with its sent or failed status exists.
