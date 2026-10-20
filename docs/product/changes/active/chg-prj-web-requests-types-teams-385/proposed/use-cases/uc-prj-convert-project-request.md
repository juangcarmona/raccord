---
id: UC-PRJ-CONVERT-PROJECT-REQUEST
type: use-case
title: "Turn a project request into a project"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-INITIAL-STATUS"
uses-terms:
  - "TERM-PROJECT-REQUEST"
provenance:
  source: ".private/research/help-center/articles/4403455031186.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A request becomes a normal project.

## Trigger

The company decides to act on a received request.

## Preconditions

The request is in status Received.

## Main Flow

1. The coordinator reviews the requested equipment and times and continues.
2. The coordinator chooses the client and location from the contact database or creates them.
3. The coordinator checks every requested equipment line against the catalogue.
4. The coordinator creates the project in Inquiry status; the request becomes Converted.

## Alternative Flows

- The coordinator declines instead: see UC-PRJ-DECLINE-PROJECT-REQUEST.

## Failure Conditions

A request without a client cannot be turned into a project.

## Postconditions

A project exists and follows the normal flow.
