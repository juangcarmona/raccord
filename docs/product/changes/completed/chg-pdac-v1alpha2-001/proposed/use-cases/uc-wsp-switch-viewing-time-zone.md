---
id: UC-WSP-SWITCH-VIEWING-TIME-ZONE
type: use-case
title: "View and enter times in another time zone"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-VIEWING-ZONE-PERSONAL"
  - "BR-WSP-ENTRY-IN-VIEWING-ZONE"
  - "BR-WSP-DST-CONVERSION"
uses-terms:
  - "TERM-VIEWING-TIME-ZONE"
provenance:
  source: ".private/research/help-center/articles/115003579769.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The planner sees and edits a project from the perspective of another time zone.

## Trigger

The planner works on an event in another region.

## Preconditions

- Multiple time zone support is enabled.

## Main Flow

1. The planner searches a city in the time zone selector and selects it.
2. The product refreshes and shows all times in that zone for this planner only.

## Alternative Flows

- None.

## Failure Conditions

- Multiple time zone support is not enabled: switching the viewing zone is refused and times keep showing in the default zone (BR-WSP-VIEWING-ZONE-PERSONAL).
- The searched city matches no time zone: nothing is selected and the current zone stays.
- Times typed while viewing in another zone are read in that viewing zone, not the project's, so entries in the wrong zone are a risk the planner must check (BR-WSP-ENTRY-IN-VIEWING-ZONE).
- A time that does not exist or occurs twice at a summer or winter time change is converted using that zone's rules (BR-WSP-DST-CONVERSION).

## Postconditions

- The planner's view and time entry use the selected zone.
