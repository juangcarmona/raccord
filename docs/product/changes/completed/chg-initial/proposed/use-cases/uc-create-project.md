---
id: UC-CREATE-PROJECT
type: use-case
title: "Create a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by: []
uses-terms:
  - "TERM-PROJECT"
  - "TERM-CUSTOMER"
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
---

## Goal

A job is recorded for a customer and period.

## Trigger

A job is commissioned.

## Preconditions

The customer is registered.

## Main Flow

1. The coordinator names the project, chooses the customer and enters the planning period and the usage period.
2. The product checks that the usage period lies within the planning period and saves the project in Draft.

## Alternative Flows

None.

## Failure Conditions

- A missing customer or name is refused.
- A period whose end is before its start, or a usage period outside the planning period, is refused.

## Postconditions

Billable work can be recorded on the project.
