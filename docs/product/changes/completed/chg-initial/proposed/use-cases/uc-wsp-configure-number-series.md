---
id: UC-WSP-CONFIGURE-NUMBER-SERIES
type: use-case
title: "Configure number series"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-SERIES-CURRENT-NUMBER"
uses-terms:
  - "TERM-NUMBER-SERIES"
provenance:
  source: ".private/research/help-center/articles/15426355918226.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

Records and documents are numbered the way the company wants.

## Trigger

Onboarding or a new numbering policy.

## Preconditions

- The administrator has configuration access.

## Main Flow

1. The administrator opens the number series.
2. The administrator adjusts the format or current number of a series and saves.

## Alternative Flows

- None.

## Failure Conditions

- The user lacks configuration access: the change is refused and the series is unchanged.
- The format or current number is invalid, for example not matching the series format: it is refused with the reason and nothing is saved (BR-WSP-SERIES-CURRENT-NUMBER).
- Another administrator changed the series, or an item was created in it, in the meantime: the save is refused and the user must reload and retry.

## Postconditions

- New items are numbered from the adjusted series.
