---
id: UC-WHS-SCAN-RETURN
type: use-case
title: "Return equipment from several projects by scanning"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-SCAN-RETURN-ROUTING"
  - "BR-WHS-EARLY-RETURN-RELEASES"
uses-terms:
  - "TERM-SCAN-RETURN"
  - "TERM-CHECK-IN"
provenance:
  source: ".private/research/help-center/articles/13959262021394.md, .private/research/help-center/articles/360021519799.md, .private/research/help-center/articles/15125682136850.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Mixed returns are booked back without sorting per project.

## Trigger

A truck with gear from several jobs is unloaded.

## Preconditions

Equipment carries codes.

## Main Flow

1. The technician starts scan return.
2. For each scanned article the technician selects the project and the quantity returned.
3. The technician may add a note per row.

## Alternative Flows

None.

## Failure Conditions

None.

## Postconditions

Returned equipment booked back.
