---
id: UC-WSP-SET-TIME-ZONE
type: use-case
title: "Configure workspace time zones"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-DEFAULT-TIME-ZONE-BASELINE"
  - "BR-WSP-FORMATS-BY-REGION"
uses-terms:
  - "TERM-WORKSPACE-TIME-ZONE"
  - "TERM-MULTIPLE-TIME-ZONES"
  - "TERM-PROJECT-TIME-ZONE"
provenance:
  source: ".private/research/help-center/articles/115003579769.md, .private/research/help-center/articles/15426355918226.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Planning times are correct for where work happens.

## Trigger

The company works in a different zone than detected, or across several zones.

## Preconditions

- The administrator has configuration access.

## Main Flow

1. The administrator sets the default time zone.
2. Optionally the administrator enables multiple time zone support and saves.
3. Planners can then give each project its own time zone.

## Alternative Flows

- Single-zone company: all projects are planned in the default zone.

## Failure Conditions

- The user lacks configuration access: the time zone settings are refused and the current ones stay.
- An unknown time zone is chosen: it is refused with the reason and nothing is saved.
- Another administrator changed the settings in the meantime: the save is refused and the user must reload and retry.
- Changing the default time zone applies to the whole workspace, so the administrator must expect existing views to shift (BR-WSP-DEFAULT-TIME-ZONE-BASELINE).

## Postconditions

- Times across the workspace are expressed in the configured zones.
