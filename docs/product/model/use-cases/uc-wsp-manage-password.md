---
id: UC-WSP-MANAGE-PASSWORD
type: use-case
title: "Set, change or reset a profile password"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-BREACHED-PASSWORD-CHECK"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-EXTERNAL-SIGN-IN"
  - "TERM-BREACHED-PASSWORD-WARNING"
provenance:
  source: ".private/research/help-center/articles/15867463701394.md, .private/research/help-center/articles/360014997360.md, .private/research/help-center/articles/4402790420882.md, .private/research/help-center/articles/31560381502226.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

The person controls the password of their profile.

## Trigger

The person forgot the password, wants a new one, or has only an external sign-in and needs a password.

## Preconditions

- A personal profile exists.

## Main Flow

1. The person opens the password section of their profile settings.
2. The person enters a new password and saves.
3. The product stores it; subsequent sign-ins accept it.

## Alternative Flows

- Forgotten password: the person requests a reset when signing in and receives a reset link by email.

## Failure Conditions

- The new password appears in breach lists: the product warns before or after saving.

## Postconditions

- The profile can be signed into with the new password.
