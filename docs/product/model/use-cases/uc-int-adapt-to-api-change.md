---
id: UC-INT-ADAPT-TO-API-CHANGE
type: use-case
title: "Adapt an integration to a public-interface change"
status: draft
primary-actor: "ACT-EXTERNAL-APPLICATION"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-INT-DEPRECATION-TRANSITION"
uses-terms:
  - "TERM-API-VERSION"
  - "TERM-API-DEPRECATION"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An integration keeps working when the interface evolves.

## Trigger

A new interface version announces a moved, renamed or removed field.

## Preconditions

The integration uses the affected field.

## Main Flow

1. The product publishes the version notes with the change and the required action.
2. During the transition period the product returns the field in both its old and new place with the same values.
3. The integration owner switches to the new place.
4. After the transition period the old place is removed.

## Alternative Flows

Breaking change without transition: flagged as breaking in the notes.

## Failure Conditions

Integrations not updated in time lose the old field after the transition period.

## Postconditions

The integration reads the field from its new place.
