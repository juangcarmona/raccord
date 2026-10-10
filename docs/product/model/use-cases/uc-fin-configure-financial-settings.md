---
id: UC-FIN-CONFIGURE-FINANCIAL-SETTINGS
type: use-case
title: "Configure financial document settings"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-QUOTE-VALIDITY-DEFAULT"
  - "BR-FIN-SIGN-AFTER-EXPIRY"
  - "BR-FIN-TERMS-ATTACHMENT"
  - "BR-FIN-CREDIT-NOTE-SERIES"
uses-terms:
  - "TERM-QUOTE-VALIDITY"
  - "TERM-TERMS-AND-CONDITIONS"
  - "TERM-ADDITIONAL-CONDITIONS"
  - "TERM-INVOICE-SERIES"
provenance:
  source: ".private/research/help-center/articles/360015935839.md, .private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/360013938099.md, .private/research/help-center/articles/6819935599378.md, .private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Financial documents carry the right validity, conditions and numbering.

## Trigger

The company prepares for quoting and invoicing or changes its policies.

## Preconditions

None.

## Main Flow

1. The administrator sets the default quote and contract validity in days and whether customers may sign after expiry.
2. The administrator uploads the terms and conditions PDF and chooses for quotes, contracts and invoices whether it is sent.
3. The administrator creates reusable additional conditions and assigns defaults to project types.
4. The administrator configures number series for quotes, contracts, invoices and credit notes and their defaults per project type.

## Alternative Flows

None.

## Failure Conditions

A non-PDF terms file is refused.

## Postconditions

New documents use these settings.
