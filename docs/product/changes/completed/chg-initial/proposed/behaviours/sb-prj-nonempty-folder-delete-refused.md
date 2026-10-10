---
id: SB-PRJ-NONEMPTY-FOLDER-DELETE-REFUSED
type: structured-behaviour
title: "A folder with contacts cannot be deleted"
status: draft
illustrates:
  - "BR-PRJ-FOLDER-DELETE-NONEMPTY"
given:
  - "Contact folder 'Venues' holds 3 contacts"
when: "The user deletes 'Venues'"
then:
  - "The deletion is refused"
  - "The folder and contacts remain"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013700660.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Records always have a folder.

## Boundaries

None.
