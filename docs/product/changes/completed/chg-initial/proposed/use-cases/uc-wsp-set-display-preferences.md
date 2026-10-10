---
id: UC-WSP-SET-DISPLAY-PREFERENCES
type: use-case
title: "Adjust personal display"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by: []
uses-terms:
  - "TERM-COMPACT-MODE"
provenance:
  source: ".private/research/help-center/articles/15743843741970.md, .private/research/help-center/articles/360017016060.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The product's presentation fits the person's device and working style.

## Trigger

The person wants denser lists or wants to learn shortcuts.

## Preconditions

- The person is signed in.

## Main Flow

1. The person opens display settings and switches compact mode on or off.
2. The product condenses or restores list density for that person.

## Alternative Flows

- The person shows the list of keyboard shortcuts, which adapts to what the person is doing.

## Failure Conditions

- The person is not signed in: the preference is refused and nothing is saved.
- The preference cannot be saved because the product is unreachable: the failure is shown and the setting stays as it was until retried.

## Postconditions

- The display matches the person's choice.
