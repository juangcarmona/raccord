---
id: UC-CRW-CONFIGURE-TIME-REGISTRATION
type: use-case
title: "Configure time registration"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-TIME-REGISTRATION-RIGHTS"
  - "BR-CRW-MINIMUM-PAID-HOURS-CHECK"
uses-terms:
  - "TERM-TIME-REGISTRATION-RIGHTS"
  - "TERM-MINIMUM-PAID-HOURS"
provenance:
  source: ".private/research/help-center/articles/360015498839.md, .private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Registration works according to company policy.

## Trigger

Introducing time registration.

## Preconditions

The user may change configuration.

## Main Flow

1. The administrator sets role rights for own and others' hours, pre-registration, activities and clock-in from the mobile web app.
2. The administrator sets days back and the default break.
3. Optionally the administrator sets minimum paid hours and enables approval on the worked hour type.

## Alternative Flows

None.

## Failure Conditions

- The user lacks the right to change configuration: the settings are refused and the current policy stays in force.
- An invalid value, such as a negative number of days back, break or minimum paid hours, is entered: the settings are refused with the reason and nothing is saved.
- Another administrator changed the configuration in the meantime: the save is refused and the user must reload and retry.
- When minimum paid hours are not configured, registrations are not flagged for being short (BR-CRW-MINIMUM-PAID-HOURS-CHECK).

## Postconditions

Registration follows the settings.
