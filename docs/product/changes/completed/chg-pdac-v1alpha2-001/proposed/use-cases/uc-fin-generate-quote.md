---
id: UC-FIN-GENERATE-QUOTE
type: use-case
title: "Generate a quote or contract"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-QUOTE-SNAPSHOTS-PLAN"
  - "BR-FIN-QUOTE-CONTRACT-SAME"
  - "BR-FIN-QUOTE-VALIDITY-DEFAULT"
  - "BR-FIN-DEFAULTS-BY-PROJECT-TYPE"
uses-terms:
  - "TERM-QUOTE"
  - "TERM-CONTRACT"
  - "TERM-QUOTE-VALIDITY"
  - "TERM-COMMERCIAL-DOCUMENT-NUMBER"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/29481848461458.md, .private/research/help-center/articles/360013264720.md, .private/research/help-center/articles/360015935839.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The customer can receive a priced offer for the planned job.

## Trigger

The customer asks for an offer.

## Preconditions

The project has planned equipment, functions or costs; financial settings (VAT, ledgers, factors, terms) are prepared.

## Main Flow

1. The coordinator chooses quote or contract and starts a new document.
2. The product proposes number series and number, expiration date from the default validity, template and letterhead from the project type.
3. The coordinator optionally adds a personal text and changes template, letterhead or the display of kit contents.
4. The product copies the plan prices into the document, stores it with the project and among its files.

## Alternative Flows

Alternatives: the coordinator creates an additional quote with a new number to offer the customer options; all appear in the project.

## Failure Conditions

No priced content: the document contains no prices.

## Postconditions

A current quote or contract version exists with its validity.
