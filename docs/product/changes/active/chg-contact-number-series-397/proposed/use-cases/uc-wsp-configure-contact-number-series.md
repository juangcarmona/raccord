---
id: UC-WSP-CONFIGURE-CONTACT-NUMBER-SERIES
type: use-case
title: Configure the contact-number series
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION"
uses-terms:
  - "TERM-CONTACT-NUMBER"
---

## Goal

New contacts receive numbers that follow the workspace's configured contact-number sequence.

## Trigger

An administrator sets up the workspace or changes the contact-number policy.

## Preconditions

- The administrator has workspace configuration access.

## Main Flow

1. The administrator opens the contact-number series settings.
2. The product shows the current prefix and current number.
3. The administrator changes the prefix or current number and saves.
4. The product validates and stores the settings for future contact creation.

## Alternative Flows

- The administrator leaves the prefix empty to use plain decimal contact numbers.

## Failure Conditions

- A user without workspace configuration access cannot change the series.
- An invalid prefix or current number is rejected without changing the active settings.
- If another administrator changes the settings concurrently, the save is rejected and the administrator must reload and retry.

## Postconditions

Future contacts without an explicitly supplied number receive the next available number from the updated series; existing contact numbers remain unchanged.
