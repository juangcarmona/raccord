---
id: UC-WHS-REVIEW-CROSS-DOCKING
type: use-case
title: "Review cross-docking opportunities"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-CROSS-DOCK-ELIGIBILITY"
uses-terms:
  - "TERM-CROSS-DOCKING-OVERLAP"
  - "TERM-WAREHOUSE-NOTE"
provenance:
  source: ".private/research/help-center/articles/11884262854930.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Returning equipment needed soon is staged, not shelved.

## Trigger

Planning the next hours of warehouse work.

## Preconditions

Confirmed projects with shared articles.

## Main Flow

1. The user opens the overview with a 12, 24 or 48 hour window, filters by folder and saved filters.
2. The product lists articles with code, name, returning, overlap and leaving quantities.
3. The user opens details per project or a timeline per article.
4. The user leaves a note for the crew with visibility and display places.

## Alternative Flows

- None.

## Failure Conditions

- No article meeting the window and overlap conditions yields an empty overview (BR-WHS-CROSS-DOCK-ELIGIBILITY).
- A user without access to the overview is refused and sees nothing.
- A note without text is refused with the reason and nothing is saved.
- If the note was affected by another user's change in the meantime, it is refused and the user must reload and retry.

## Postconditions

Crew knows what to keep at the dock.
