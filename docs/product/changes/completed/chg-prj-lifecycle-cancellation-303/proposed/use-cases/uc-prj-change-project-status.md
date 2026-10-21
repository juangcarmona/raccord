---
id: UC-PRJ-CHANGE-PROJECT-STATUS
type: use-case
title: "Change a project status manually"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-MANUAL-STATUS-ANY-TIME"
  - "BR-PRJ-FIXED-STATUS-SET"
  - "BR-WHS-STATUS-INDEPENDENCE"
  - "BR-PRJ-STATUS-AREA-VISIBILITY"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The project status reflects reality when automatic transitions did not.

## Trigger

The coordinator learns of a confirmation, preparation, return or other change by other means.

## Preconditions

The project exists.

## Main Flow

1. The coordinator sets a new status on the project.
2. The product applies the status effects (warehouse eligibility, invoicing eligibility, availability, overview presentation).

## Alternative Flows

- Status changed from warehouse handling by completing a stage for the project.
- New status is Cancelled: continue with UC-PRJ-CANCEL-PROJECT.

## Failure Conditions

Complete blocks operational edits and new planning until the project is explicitly reopened. Other lifecycle transitions follow the documented project-status rules.

## Postconditions

The new status is shown; equipment items keep their own warehouse statuses.
