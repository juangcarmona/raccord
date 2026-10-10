---
id: UC-CRW-ARCHIVE-CREW-RATE
type: use-case
title: "Archive a crew rate"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-RATE-ARCHIVE-PRECONDITION"
uses-terms:
  - "TERM-CREW-RATE-STATUS"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Outdated rates are retired without losing history.

## Trigger

A tariff is replaced.

## Preconditions

The rate is not linked to crew members and not used on future functions or shifts.

## Main Flow

1. The user selects the rate and archives it.
2. The product confirms and moves it to the archived rates.

## Alternative Flows

Archived rates are listed separately for reference.

## Failure Conditions

The rate is still in use on future work: archiving is refused until those functions are changed.

## Postconditions

The rate is no longer selectable; past functions show it struck through.
