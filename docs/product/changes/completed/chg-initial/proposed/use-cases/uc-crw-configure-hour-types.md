---
id: UC-CRW-CONFIGURE-HOUR-TYPES
type: use-case
title: "Configure hour types"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-APPROVAL-ROUTING"
  - "BR-CRW-CORRECTION-TYPE-OBSOLETE"
  - "BR-CRW-PAYROLL-CODE-IN-EXPORT"
uses-terms:
  - "TERM-HOUR-TYPE"
provenance:
  source: ".private/research/help-center/articles/24569529468562.md, .private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Time categories behave as company policy requires.

## Trigger

Introducing a new category of worked time.

## Preconditions

The user may change configuration.

## Main Flow

1. The administrator edits a default hour type or adds an hour type.
2. The administrator sets name, colour, payroll code and approval.
3. The product saves.

## Alternative Flows

None.

## Failure Conditions

A type without a name is refused.

## Postconditions

Time of this type follows the settings.
