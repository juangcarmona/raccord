---
id: UC-CRW-DECLARE-OWN-AVAILABILITY
type: use-case
title: "Declare one's own availability"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-AVAILABILITY-RIGHTS"
  - "BR-CRW-SELF-PRESELECTED"
  - "BR-CRW-RECURRENCE-SERIES"
uses-terms:
  - "TERM-AVAILABILITY-ENTRY"
provenance:
  source: ".private/research/help-center/articles/360014685859.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The crew member tells planners when they can work.

## Trigger

The crew member plans their time.

## Preconditions

The role allows entering own availability.

## Main Flow

1. The crew member selects dates in their schedule, on the web or in the mobile web app.
2. They mark themselves available, unavailable or unknown, optionally recurring with a remark.
3. The product records it.

## Alternative Flows

None.

## Failure Conditions

Without the right the option is absent.

## Postconditions

Planners see the declared availability.
