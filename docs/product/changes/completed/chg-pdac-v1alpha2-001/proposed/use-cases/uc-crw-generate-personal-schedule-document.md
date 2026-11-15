---
id: UC-CRW-GENERATE-PERSONAL-SCHEDULE-DOCUMENT
type: use-case
title: "Generate personal schedule documents"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-SCHEDULE-PERSONAL-CONTENT"
uses-terms:
  - "TERM-PERSONAL-SCHEDULE-DOCUMENT"
provenance:
  source: ".private/research/help-center/articles/360014672919.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Crew receive a formal document of their planning.

## Trigger

A document is preferred over notifications.

## Preconditions

Crew have planning in the period.

## Main Flow

1. The planner chooses template, letterhead and period.
2. The planner selects crew members and options.
3. The product generates one document per crew member, which can be downloaded, emailed or printed.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to produce schedule documents: generation is refused and nothing is produced.
- A selected crew member has no planning in the period: no document is produced for that person and the planner is told so (BR-CRW-SCHEDULE-PERSONAL-CONTENT).
- No template, period or crew member is chosen: generation is refused with the reason.
- Emailing a document fails because the mail service is unreachable: the failure is shown and the sending can be retried.

## Postconditions

Documents exist.
