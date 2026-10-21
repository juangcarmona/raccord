---
id: TERM-VAT-SCHEME
type: domain-term
title: "VAT scheme"
status: active
defined-in: "BC-INVOICING"
synonyms: []
uses-terms:
  - "TERM-VAT-CLASS"
  - "TERM-VAT-SCHEME-KIND"
  - "TERM-TAX-CATEGORY-CODE"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648360.md, .private/research/help-center/articles/360014032939.md, .private/research/help-center/articles/360019360320.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A named mapping that determines the VAT percentage applied to each VAT class, or a single fixed rate, or reverse charge. One scheme is the default; customers, projects and invoices can use another. Schemes also carry e-invoice tax category codes and accounting codes per rate. A rate that has been used cannot be changed.

## Distinguish From

VAT class; VAT breakdown on an invoice (TERM-VAT-BREAKDOWN).

## Usage

Selected on projects and invoices; new schemes are created when legal rates change.
