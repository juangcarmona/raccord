---
id: UC-PRJ-DECLINE-PROJECT-REQUEST
type: use-case
title: "Decline a project request"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-REQUEST-DECLINE-NOTICE"
uses-terms:
  - "TERM-PROJECT-REQUEST-STATUS"
provenance:
  source: ".private/research/help-center/articles/4403455031186.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An unwanted request is closed and the person who raised it informed.

## Trigger

The company cannot or will not fulfil a request.

## Preconditions

The request is Received and no project has been created from it.

## Main Flow

1. The coordinator declines the request.
2. The product sets it to Declined and e-mails the staff member who created it.

## Alternative Flows

None.

## Failure Conditions

After conversion the request can no longer be declined.

## Postconditions

The requester knows.
