---
id: UC-FIN-ANALYZE-STATISTICS
type: use-case
title: "Analyse results with statistics views"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-STATISTICS-ARE-LIVE"
  - "BR-FIN-CHART-TOP-TEN"
  - "BR-FIN-USAGE-PERCENTAGE"
uses-terms:
  - "TERM-STATISTICS-VIEW"
  - "TERM-USAGE-PERCENTAGE"
provenance:
  source: ".private/research/help-center/articles/360013767499.md, .private/research/help-center/articles/18431827595666.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Management understands revenue, margins, payment behaviour and utilisation across many records.

## Trigger

A business question needs data across projects or resources.

## Preconditions

The user has financial visibility.

## Main Flow

1. The user creates a statistics view choosing a type, name and description.
2. The user chooses columns, period and filters.
3. The product computes figures from current data.
4. The user saves the view and optionally a pie or bar chart with chosen data sources and filters.

## Alternative Flows

The user exports the results or downloads the chart as an image.

## Failure Conditions

- A user without financial visibility is refused the view and nothing is shown or saved.
- Filters or a period that match no records produce an empty result and an empty chart.
- Figures reflect current data at the moment of viewing, so past periods change when underlying data changes (BR-FIN-STATISTICS-ARE-LIVE).
- Saving a view without a name or type is refused with the reason and nothing is saved.

## Postconditions

The view is saved for later use.
