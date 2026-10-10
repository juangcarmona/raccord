---
id: UC-PRJ-CONFIGURE-DASHBOARD
type: use-case
title: "Configure a personal dashboard"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-DASHBOARD-DEFAULTS"
  - "BR-PRJ-REVENUE-DAY-SPREAD"
  - "BR-PRJ-OPEN-INVOICE-BUCKETS"
  - "BR-PRJ-REVENUE-WIDGET-RANGE"
uses-terms:
  - "TERM-DASHBOARD-WIDGET"
provenance:
  source: ".private/research/help-center/articles/360014243139.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Each user sees what needs doing and what is coming.

## Trigger

A user wants a personal start page.

## Preconditions

The user's role allows dashboard editing.

## Main Flow

1. The user creates, renames or deletes named dashboards.
2. The user adds, places, resizes, configures and removes widgets.
3. The product saves; widgets link to their detail views.

## Alternative Flows

None.

## Failure Conditions

Without dashboard rights widgets cannot be edited.

## Postconditions

The dashboard shows the chosen widgets.
