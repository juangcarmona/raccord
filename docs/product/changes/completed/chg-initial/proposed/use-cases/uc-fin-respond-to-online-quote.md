---
id: UC-FIN-RESPOND-TO-ONLINE-QUOTE
type: use-case
title: "Review, sign or request changes to an online quote"
status: draft
primary-actor: "ACT-CUSTOMER"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-ONLINE-QUOTE-ACCESS"
  - "BR-FIN-ONLINE-QUOTE-VIEWS"
  - "BR-FIN-SIGN-AFTER-EXPIRY"
  - "BR-FIN-NEW-SIGNATURE-NEW-NUMBER"
uses-terms:
  - "TERM-ONLINE-QUOTE"
  - "TERM-SIGNED-QUOTE"
  - "TERM-QUOTE-CHANGE-REQUEST"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md, .private/research/help-center/articles/360013938099.md, .private/research/help-center/articles/360012594659.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

The customer accepts the offer or asks for changes without handling files.

## Trigger

The customer receives the link.

## Preconditions

The quote is published.

## Main Flow

1. The customer opens the link and enters the password.
2. The product counts the view and shows the latest synchronised version and earlier versions.
3. The customer signs the quote, seeing the terms and conditions.
4. The product records the signature.

## Alternative Flows

The customer downloads the quote or shares the link; the customer requests changes, which appear to the company as an open change request.

## Failure Conditions

Expired quote and signing after expiry not allowed: signing is refused. Wrong password: the quote is not shown.

## Postconditions

The quote is signed, or a change request is open.
