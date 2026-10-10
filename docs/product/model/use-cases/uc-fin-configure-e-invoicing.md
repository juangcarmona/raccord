---
id: UC-FIN-CONFIGURE-E-INVOICING
type: use-case
title: "Configure e-invoicing"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-E-INVOICE-IDENTIFIERS"
  - "BR-FIN-E-INVOICE-AUTO-ATTACH"
  - "BR-FIN-TAX-CATEGORY-PER-RATE"
uses-terms:
  - "TERM-ELECTRONIC-IDENTIFIER"
  - "TERM-TAX-CATEGORY-CODE"
  - "TERM-E-INVOICE-FORMAT"
provenance:
  source: ".private/research/help-center/articles/360014032939.md, .private/research/help-center/articles/31583505898898.md, .private/research/help-center/articles/360019360320.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Invoices can be produced as valid structured e-invoices.

## Trigger

Customers or law require e-invoices.

## Preconditions

VAT schemes exist.

## Main Flow

1. The administrator enters the company identification number and scheme and bank account.
2. The administrator records the tax category code of each VAT rate.
3. The administrator enters identification number and scheme on customers.
4. The administrator enables automatic attachment of the structured file.

## Alternative Flows

None.

## Failure Conditions

- A VAT rate without its standard tax category code cannot be used in e-invoices, and the gap is reported (BR-FIN-TAX-CATEGORY-PER-RATE).
- An e-invoice for a company or customer lacking identification number and scheme is flagged (BR-FIN-E-INVOICE-IDENTIFIERS).
- An actor without permission to change company settings is refused and nothing changes.

## Postconditions

E-invoices can be exported.
