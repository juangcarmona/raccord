---
id: UC-WSP-SET-LANGUAGE
type: use-case
title: "Set workspace or personal language"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-LANGUAGE-SCOPES"
uses-terms:
  - "TERM-WORKSPACE-LANGUAGE"
  - "TERM-PROFILE-LANGUAGE"
provenance:
  source: ".private/research/help-center/articles/360014284880.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Users and messages use the right language.

## Trigger

The company or a user prefers another language.

## Preconditions

- None.

## Main Flow

1. The administrator selects the workspace language in the display settings.
2. The product uses it for the interface default, notifications, defaults and provided templates.

## Alternative Flows

- A user changes only their profile language in account settings; their screens change after signing in again.
- A user changes the mobile web app language in the app settings.

## Failure Conditions

- A language that is only partially translated shows untranslated parts in the base language.

## Postconditions

- The chosen languages are in effect.
