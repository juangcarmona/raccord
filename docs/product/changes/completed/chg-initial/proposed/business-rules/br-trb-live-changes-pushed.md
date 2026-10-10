---
id: "BR-TRB-LIVE-CHANGES-PUSHED"
type: business-rule
title: "Saved changes appear on colleagues' screens without reloading"
status: draft
applies-to:
  - "UC-TRB-SEE-LIVE-CHANGES"
uses-terms:
  - "TERM-LIVE-UPDATE-CONNECTION"
provenance:
  source: ".private/research/help-center/articles/360013988420.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a user saves a change, the product pushes it through the live update connection to every other user who has the affected data open, whose screens update without a reload.

## Rationale

Teams plan the same projects simultaneously and need current data.

## Examples

See SB-TRB-COLLEAGUE-CHANGE-APPEARS.

## Exceptions

Users whose live connection is lost do not receive the change until they refresh.
