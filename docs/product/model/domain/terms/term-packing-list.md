---
id: TERM-PACKING-LIST
type: domain-term
title: "Packing list"
status: draft
defined-in: BC-WAREHOUSE
synonyms: []
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-READY-TO-PACK"
  - "TERM-WAREHOUSE-DUE-DATE"
  - "TERM-PLANNED-EQUIPMENT"
  - "TERM-PACKING-SLIP"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md, .private/research/help-center/articles/360014458519.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The unit of warehouse work: all planned lines of one project that leave together at one pickup moment, or come back together at one return moment. A project has one packing list per distinct pickup or return date of its lines. A packing list carries a warehouse status, a due date, its project, notes, and the booked quantities per status.

## Distinguish From

Not the project: a project with staggered equipment times has several packing lists. Not the packing slip, which is its printed document.

## Usage

Shown as a card on the warehouse board once the project is ready to pack.
