---
id: UC-WSP-CREATE-WORKSPACE
type: use-case
title: "Set up the company installation"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-BREACHED-PASSWORD-CHECK"
  - "BR-WSP-DEFAULT-TIME-ZONE-BASELINE"
uses-terms:
  - "TERM-WORKSPACE"
  - "TERM-SAMPLE-DATA"
  - "TERM-WORKSPACE-LANGUAGE"
  - "TERM-PERSONAL-PROFILE"
provenance:
  source: ".private/research/help-center/articles/360015603179.md, .private/research/help-center/articles/4402790420882.md"
  confidence: medium
  recovered-from: documentation
---
## Goal
The company starts working in its own installation.
## Trigger
The company has installed the product and opens it for the first time.
## Preconditions
- The product is installed and reachable.
## Main Flow
1. The person creating the first account enters their email address and a password, or chooses a Microsoft or Google account.
2. The person enters the company name, language and a phone number.
3. The product sets up the installation with sample data, sets its default time zone from the location and gives the person the Admin role.
4. The product optionally asks onboarding questions, which may be skipped.
## Alternative Flows
- None.
## Failure Conditions
- The company name, language or phone number is missing or invalid: the set-up is refused with the reason and nothing is created.
- The password is found in public breach lists: a warning recommends changing to a unique password (BR-WSP-BREACHED-PASSWORD-CHECK).
- The location needed to derive the default time zone cannot be determined: the installation falls back to a baseline time zone that can be changed later (BR-WSP-DEFAULT-TIME-ZONE-BASELINE).
- The product cannot complete the set-up: it fails visibly, nothing partial is left and it can be retried.
## Postconditions
- The installation exists with sample data and the first person as administrator.
