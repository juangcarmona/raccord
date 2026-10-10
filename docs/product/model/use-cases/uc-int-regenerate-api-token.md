---
id: UC-INT-REGENERATE-API-TOKEN
type: use-case
title: "Regenerate the API token"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-EXTERNAL-APPLICATION"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-INT-REGENERATION-REVOKES-TOKEN"
uses-terms:
  - "TERM-API-TOKEN"
  - "TERM-API-TOKEN-VALIDITY"
provenance:
  source: ".private/research/help-center/articles/360013767839.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A possibly exposed or expired token stops working and a new one replaces it.

## Trigger

The token was shared, shown in a screenshot or recording, or has expired.

## Preconditions

Public-interface access is connected.

## Main Flow

1. The administrator asks for a new token.
2. The product creates a new valid token and revokes the previous one immediately.
3. The administrator updates every external application with the new token.

## Alternative Flows

None.

## Failure Conditions

Applications still using the old token are refused until updated.

## Postconditions

Only the new token authenticates.
