---
id: "UC-DOC-REVIEW-COMMUNICATION-LOG"
type: use-case
title: "Review the communication log"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-LOG-SCOPE"
  - "BR-DOC-COMMUNICATION-ACCESS"
  - "BR-DOC-STATUS-EMAIL-ONLY"
uses-terms:
  - "TERM-COMMUNICATION-LOG"
  - "TERM-COMMUNICATION-TYPE"
  - "TERM-COMMUNICATION-CHANNEL"
provenance:
  source: ".private/research/help-center/articles/29923797058194.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user knows what was sent to whom, when and how.

## Trigger

A question arises whether someone was informed.

## Preconditions

The user's role permits viewing communications.

## Main Flow

1. The user opens the communication log.
2. The user filters by recipient, communication type and channel.
3. The user opens a record's details.

## Alternative Flows

None.

## Failure Conditions

Records outside the user's permissions are not shown. Filtering by project is not possible.

## Postconditions

Nothing changes.
