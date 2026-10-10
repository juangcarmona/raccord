---
id: UC-FIN-REGENERATE-QUOTE
type: use-case
title: "Regenerate a quote after project changes"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-REGENERATION-MODES"
  - "BR-FIN-NEW-SIGNATURE-NEW-NUMBER"
  - "BR-FIN-ONLINE-VERSION-VISIBILITY"
  - "BR-FIN-QUOTE-SNAPSHOTS-PLAN"
uses-terms:
  - "TERM-REGENERATION-MODE"
  - "TERM-QUOTE-VERSION"
provenance:
  source: ".private/research/help-center/articles/360012594659.md, .private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The quote reflects the changed project in the intended way.

## Trigger

The plan changed or the customer requested changes.

## Preconditions

A quote or contract exists.

## Main Flow

1. The coordinator regenerates the document and chooses a regeneration mode.
2. Overwrite: the product replaces the content, keeping number and version.
3. New version: the product raises the version number and keeps the previous version as a project file.
4. For a published quote, the product updates the online page according to the mode.

## Alternative Flows

New quote: when the customer must sign the changes, the coordinator creates a new quote with a new number instead. New version without synchronising: the online page keeps the old version until synchronised.

## Failure Conditions

No regeneration: the document keeps its earlier content.

## Postconditions

The document shows the current plan in the chosen identity.
