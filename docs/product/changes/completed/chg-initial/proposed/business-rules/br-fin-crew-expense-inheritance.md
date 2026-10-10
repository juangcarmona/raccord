---
id: BR-FIN-CREW-EXPENSE-INHERITANCE
type: business-rule
title: "Crew expenses are inherited from default function to planned function to crew member"
status: draft
applies-to:
  - "UC-FIN-MANAGE-CREW-EXPENSES"
uses-terms:
  - "TERM-CREW-EXPENSE"
provenance:
  source: ".private/research/help-center/articles/31405085792146.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Expenses of a default function are copied when it is planned; changing a planned function's expenses affects only that project; expenses set on a planned crew member override those of the function for that person in that project only; editing a default function affects later plannings only.

## Rationale

Standard allowances with local exceptions.

## Examples

See SB-FIN-CREW-EXPENSE-OVERRIDE.

## Exceptions

Expenses cannot be copied between default functions nor set as defaults per crew member.
