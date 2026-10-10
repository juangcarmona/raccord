---
id: CON-FIN-E-INVOICE-STRUCTURED
type: constraint
title: "E-invoices must be structured data"
status: draft
applies-to:
  - "BC-INVOICING"
uses-terms:
  - "TERM-E-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360014032939.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

An invoice counts as an e-invoice only in a structured machine-readable format (structured XML following the standard required by the Spanish rules); plain PDF or word-processor documents do not qualify. Spanish rules require e-invoices between businesses, and the electronic identifiers of both parties are needed.

## Rationale

Imposed by Spanish e-invoicing regulation.

## Consequences

Structured export formats, tax category codes and electronic identifiers become mandatory data.
