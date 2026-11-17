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
  - "BR-WSP-NEW-USER-DEFAULT-ROLE"
uses-terms:
  - "TERM-WORKSPACE"
  - "TERM-SAMPLE-DATA"
  - "TERM-WORKSPACE-LANGUAGE"
  - "TERM-PERSONAL-PROFILE"
  - "TERM-PREDEFINED-ROLE"
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

1. While no user exists, a visitor is routed to an accessible first-run setup screen and enters an email address and password to create the first account.
2. The product creates the first account with the Admin role and makes the four predefined roles available. Company name, language and phone number are not prerequisites for this account creation.
3. In subsequent company onboarding, the administrator enters the company name, language and a phone number.
4. The product sets up the installation with sample data and sets its default time zone from the location.
5. The product optionally asks onboarding questions, which may be skipped.

## Alternative Flows

- The person creating the first account may choose a Microsoft or Google account.

## Failure Conditions

- A user already exists: first-account setup is unavailable and an attempt is refused without creating another account.
- The company name, language or phone number is missing or invalid: the subsequent company set-up is refused with the reason and no partial company set-up is created.
- The password is found in public breach lists: a warning recommends changing to a unique password (BR-WSP-BREACHED-PASSWORD-CHECK).
- The location needed to derive the default time zone cannot be determined: the installation falls back to a baseline time zone that can be changed later (BR-WSP-DEFAULT-TIME-ZONE-BASELINE).
- The product cannot complete a setup step: it fails visibly, nothing partial from that step is left and it can be retried.

## Postconditions

- First-account bootstrap creates the first person as administrator and makes the four predefined roles available.
- After subsequent company setup, the installation exists with sample data.
