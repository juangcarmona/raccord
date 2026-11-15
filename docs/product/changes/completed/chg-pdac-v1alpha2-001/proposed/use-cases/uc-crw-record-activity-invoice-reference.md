---
id: UC-CRW-RECORD-ACTIVITY-INVOICE-REFERENCE
type: use-case
title: "Record the invoice reference of activities"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PLANNED-COST-VISIBILITY"
uses-terms:
  - "TERM-ACTIVITY-INVOICE-REFERENCE"
  - "TERM-FREELANCER"
provenance:
  source: ".private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Activities already invoiced by crew are marked.

## Trigger

A crew member or freelancer invoices the company.

## Preconditions

Registered activities exist.

## Main Flow

1. The user selects activities.
2. The user enters the invoice reference.
3. The product stores it on the activities.

## Alternative Flows

A freelancer consults the agreed planned cost as reference for their invoice.

## Failure Conditions

- The user lacks permission to change activities: the reference is refused and nothing is stored.
- The invoice reference is empty or invalid, or no activity is selected: it is refused with the reason and nothing is saved.
- Another user changed the same activities in the meantime: the change is refused and the user must reload and retry.
- A freelancer without the right to see planned cost does not get the agreed amount shown (BR-CRW-PLANNED-COST-VISIBILITY).

## Postconditions

Activities show their invoice reference.
