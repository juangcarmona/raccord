---
id: UC-WSP-SET-APP-PREFERENCES
type: use-case
title: "Set mobile web app preferences"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by: []
uses-terms:
  - "TERM-MOBILE-APP"
provenance:
  source: ".private/research/help-center/articles/360014365179.md, .private/research/help-center/articles/115003579769.md, .private/research/help-center/articles/360014284880.md"
  confidence: high
  recovered-from: documentation
---
## Goal
The mobile web app shows language and theme as the person wants.
## Trigger
The person prefers another display.
## Preconditions
- The person is signed in to the mobile web app.
## Main Flow
1. The person opens the app settings.
2. The person sets the language and theme (dark, light or following the device).
## Alternative Flows
- None.
## Failure Conditions
- The person is not signed in: the preferences are refused and nothing is saved.
- An unknown language or theme is chosen: it is refused with the reason and the previous setting stays.
- The preferences cannot be saved because the product is unreachable: the failure is shown and it can be retried.
## Postconditions
- The mobile web app uses the chosen settings.
