---
id: "UC-DOC-HIDE-ITEMS-ON-FINANCIAL-DOCUMENTS"
type: use-case
title: "Keep items off customer documents"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-GROUP-PRICE-VISIBILITY"
  - "BR-DOC-HIDE-ZERO-PRICED"
  - "BR-DOC-COMBINATION-CONTENT-DISPLAY"
uses-terms:
  - "TERM-EQUIPMENT-GROUP"
provenance:
  source: ".private/research/help-center/articles/360017647860.md, .private/research/help-center/articles/360017648140.md, .private/research/help-center/articles/360017647940.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Quotations and invoices show only what the customer cares about, while packing slips stay complete.

## Trigger

A project contains items not meant for one audience.

## Preconditions

The project exists.

## Main Flow

1. The user marks an equipment group as not shown in the price calculation, or relies on zero-priced lines being hidden, or sets the combination's content default.
2. The user generates the documents.

## Alternative Flows

At generation the user overrides combination content display for all combinations.

## Failure Conditions

A 100%-discounted line is hidden too when zero-priced lines are hidden.

## Postconditions

Each document shows only its audience's content.
