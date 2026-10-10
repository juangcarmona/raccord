---
id: TERM-INVOICE-MOMENT
type: domain-term
title: "Invoice moment"
status: draft
defined-in: "BC-INVOICING"
synonyms:
  - "Billing milestone schedule"
uses-terms:
  - "TERM-INVOICE-MOMENT-TRIGGER"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360017647300.md, .private/research/help-center/articles/360015935839.md, .private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A named billing schedule that splits a project's invoicing into up to three parts, each with a percentage and a trigger (after agreement, before the first day, after the last day) and a label printed on the invoice; for example 30% before and 70% after. Defaults come from the customer or project type and can be changed per project.

## Distinguish From

Payment term (time to pay after invoicing); due date.

## Usage

Drives the invoicing queue and the default percentage of partial invoices.
