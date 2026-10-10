---
id: UC-CRW-EDIT-CREW-MEMBERS
type: use-case
title: "Edit one or many crew members"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PROFILE-OWNERSHIP"
  - "BR-CRW-PLANNABLE-FLAG"
  - "BR-CRW-DEACTIVATE-RATHER-THAN-DELETE"
uses-terms:
  - "TERM-CREW-MEMBER"
  - "TERM-PROFILE-OWNERSHIP"
provenance:
  source: ".private/research/help-center/articles/360014360520.md, .private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Crew data is correct, including in bulk.

## Trigger

Details, roles, tags or planning flags change.

## Preconditions

The crew members exist; the user may edit crew.

## Main Flow

1. The planner selects one crew member, or several.
2. For several, the planner chooses which fields to change and gives the new values.
3. The product applies the values to every selected crew member.

## Alternative Flows

- Bulk change through export, edit and re-import in update mode (see UC-CRW-IMPORT-CREW-MEMBERS).
- Retiring a person: show-in-planning and login are switched off rather than deleting.

## Failure Conditions

Fields owned by a self-maintained profile cannot be changed by the planner.

## Postconditions

Selected records carry the new values.
