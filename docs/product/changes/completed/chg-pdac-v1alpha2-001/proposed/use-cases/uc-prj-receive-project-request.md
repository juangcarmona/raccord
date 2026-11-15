---
id: UC-PRJ-RECEIVE-PROJECT-REQUEST
type: use-case
title: "Create a project request"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-REQUEST-NOTIFICATION"
uses-terms:
  - "TERM-PROJECT-REQUEST"
  - "TERM-PROJECT-REQUEST-STATUS"
provenance:
  source: ".private/research/help-center/articles/4403455031186.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A staff member's request for a project reaches the company's account managers.

## Trigger

A staff member needs a project to be set up.

## Preconditions

The requester is signed in.

## Main Flow

1. The staff member creates the request with client, location, equipment and times.
2. The product e-mails all account managers, shows the request on the dashboard and raises a notification.
3. The request is listed with the other requests, in status Received.

## Alternative Flows

None.

## Failure Conditions

A request without a client is refused.

## Postconditions

The request awaits processing.
