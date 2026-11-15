---
id: UC-FIN-PUBLISH-ONLINE-QUOTE
type: use-case
title: "Publish an online quote"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CUSTOMER"
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-ONLINE-QUOTE-ACCESS"
  - "BR-FIN-ONLINE-QUOTE-EMAIL"
  - "BR-FIN-ONLINE-VERSION-VISIBILITY"
uses-terms:
  - "TERM-ONLINE-QUOTE"
  - "TERM-ONLINE-QUOTE-STATUS"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md, .private/research/help-center/articles/30841616039570.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The customer reviews the quote on a live page that always shows the latest synchronised version.

## Trigger

The coordinator prefers a shareable link over a PDF.

## Preconditions

A generated quote exists.

## Main Flow

1. The coordinator publishes the quote.
2. The coordinator adjusts title, welcome message (default from configuration), media and password protection.
3. The product publishes the page and gives its link and password.
4. The coordinator e-mails the customer; the default message contains link and password and no PDF.

## Alternative Flows

Republishing an offline quote puts the latest version online again; the coordinator may attach the PDF manually.

## Failure Conditions

- Publishing without a generated quote is refused and nothing is published.
- An actor without permission to publish quotes is refused and nothing changes.
- A visitor without the link, or without the correct password when protection is on, cannot open the quote (BR-FIN-ONLINE-QUOTE-ACCESS).
- If the page cannot be published or the e-mail cannot be delivered, the action fails visibly and can be retried (BR-FIN-ONLINE-QUOTE-EMAIL).
- A new version not synchronised stays hidden from the customer until it is synchronised (BR-FIN-ONLINE-VERSION-VISIBILITY).

## Postconditions

The online quote is published.
