---
id: SB-PRJ-CONFIDENTIAL-FOLDER-HIDDEN
type: structured-behaviour
title: "Confidential folder hidden from users without full download rights"
status: draft
illustrates:
  - "BR-PRJ-CONFIDENTIAL-FOLDER"
given:
  - "A project has a confidential folder 'Contracts'"
  - "A crew member lacks the right to download all files"
when: "The crew member opens the project's files"
then:
  - "The 'Contracts' folder and its files are not shown"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013638440.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Confidentiality is inherited.

## Boundaries

None.
