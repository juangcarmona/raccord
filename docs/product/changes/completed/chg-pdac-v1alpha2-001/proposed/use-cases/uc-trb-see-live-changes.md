---
id: "UC-TRB-SEE-LIVE-CHANGES"
type: use-case
title: "See colleagues' changes and presence live"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-TRB-LIVE-CHANGES-PUSHED"
  - "BR-TRB-PRESENCE-SHARED"
  - "BR-TRB-CONNECTION-LOST-WARNING"
uses-terms:
  - "TERM-LIVE-UPDATE-CONNECTION"
  - "TERM-RECORD-PRESENCE"
provenance:
  source: ".private/research/help-center/articles/360013988420.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Several users work in the same data at the same time without reloading and without overwriting each other.

## Trigger

A user has the application open while colleagues change data.

## Preconditions

The user is signed in through the browser and the live update connection can be established.

## Main Flow

1. The product opens the live update connection when the application is opened.
2. A colleague saves a change.
3. The product pushes the change and the user sees the new data without reloading.
4. When the user opens a project, the product shows which colleagues are working in it, and shows the user to them.

## Alternative Flows

The user reloads after a connection loss: the latest data is loaded and the live connection is reopened.

## Failure Conditions

The live update connection is blocked or drops: the product warns that colleagues' updates may have been missed and recommends refreshing; while it stays blocked, unexpected errors and stale data may occur.

## Postconditions

The user sees the colleagues' changes; presence is visible on shared projects.
