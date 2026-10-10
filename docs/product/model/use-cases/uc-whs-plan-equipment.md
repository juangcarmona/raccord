---
id: UC-WHS-PLAN-EQUIPMENT
type: use-case
title: "Plan equipment on a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
  - "BR-WHS-PLANNED-SNAPSHOT"
  - "BR-WHS-SHORTAGE-DETECTION"
  - "BR-WHS-LINE-TIMES-DEFAULT"
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
  - "TERM-PLANNED-QUANTITY"
  - "TERM-EQUIPMENT-GROUP"
  - "TERM-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/360013730520.md, .private/research/help-center/articles/23230026077586.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The project holds the articles and quantities the job needs, each checked against availability.

## Trigger

A job's equipment needs are known or change.

## Preconditions

The project exists; the catalog holds the articles.

## Main Flow

1. The coordinator finds articles by name search, catalog tags and folders, or among up to five suggestions ranked by articles often planned together, by project type, by order within the group, by customer and by venue.
2. The coordinator adds single articles, several selected articles at once, or a typed quantity, using keyboard-only entry if preferred.
3. The product creates a planned line per article in the chosen equipment group, copying the catalog data and default times.
4. The product computes availability for each line over its planning period and marks lines in shortage.

## Alternative Flows

- Import: lines are imported from a file into the project.
- Accessories and combinations: planning a combination plans its content as defined.
- Temporary article: see UC-WHS-ADD-TEMPORARY-EQUIPMENT.

## Failure Conditions

- Requested quantity exceeds availability: the line is planned anyway and shown in shortage.

## Postconditions

Planned lines exist with quantities, times and shortage indications; availability forecasts include them according to project status.
