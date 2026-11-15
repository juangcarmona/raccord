---
id: UC-FIN-MANAGE-LEDGERS
type: use-case
title: "Manage ledger accounts"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-DEFAULT-LEDGER-BY-KIND"
  - "BR-FIN-LEDGER-IMPORT-CREATES"
uses-terms:
  - "TERM-LEDGER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013878320.md, .private/research/help-center/articles/33280698329618.md, .private/research/help-center/articles/360015935839.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Revenue and costs are booked to the right accounts.

## Trigger

Setting up accounting or adding a revenue source.

## Preconditions

None.

## Main Flow

1. The administrator reviews the default ledgers.
2. The administrator adds ledgers with name and code.
3. The administrator assigns default ledgers to equipment, functions and default additional costs.

## Alternative Flows

Ledgers are changed within one project only.

## Failure Conditions

- A ledger without a name, or whose code already exists, is refused with the reason and nothing is saved.
- An actor without permission to configure accounting is refused and nothing changes.

## Postconditions

Items carry ledgers used by exports.
