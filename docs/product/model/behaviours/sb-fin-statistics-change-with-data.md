---
id: SB-FIN-STATISTICS-CHANGE-WITH-DATA
type: structured-behaviour
title: "Past statistics change when data changes"
status: draft
illustrates:
  - "BR-FIN-STATISTICS-ARE-LIVE"
given:
  - "A saved revenue view covers last year"
when: "A user edits the price of a project from last year and reopens the view"
then:
  - "The view shows last year's revenue including the edited price"
uses-terms:
  - "TERM-STATISTICS-VIEW"
provenance:
  source: ".private/research/help-center/articles/360013767499.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Statistics have no frozen history.

## Boundaries

Exported files are not changed.
