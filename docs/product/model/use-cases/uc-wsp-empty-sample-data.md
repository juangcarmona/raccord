---
id: UC-WSP-EMPTY-SAMPLE-DATA
type: use-case
title: "Remove sample data after first set-up"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-EMPTY-DATA-SCOPE"
  - "BR-WSP-EMPTY-BEFORE-PRODUCTION"
  - "BR-WSP-EMPTY-SIGNS-OUT"
uses-terms:
  - "TERM-SAMPLE-DATA"
provenance:
  source: ".private/research/help-center/articles/360013982380.md, .private/research/help-center/articles/360015603179.md, .private/research/help-center/articles/360015900179.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The company starts its real set-up from a clean database.

## Trigger

The company has finished exploring with sample data.

## Preconditions

- The workspace is not yet in production use.
- The administrator has configuration access.

## Main Flow

1. The administrator asks to empty the database and confirms the irreversible deletion.
2. The product signs everyone out and deletes the sample data, which can take up to five minutes.
3. Crew members, document templates, letterheads, settings, presets and roles remain.

## Alternative Flows

- The company already entered its own data: it deletes sample records by hand instead.

## Failure Conditions

- The workspace is in production use: automatic emptying is not offered.

## Postconditions

- The workspace contains no sample records.
