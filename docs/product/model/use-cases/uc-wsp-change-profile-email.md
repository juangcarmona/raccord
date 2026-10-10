---
id: UC-WSP-CHANGE-PROFILE-EMAIL
type: use-case
title: "Change the email address of a personal profile"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-PROFILE-EMAIL-SELF-SERVICE"
  - "BR-WSP-EMAIL-CHANGE-PROPAGATES"
  - "BR-WSP-ONE-PROFILE-PER-EMAIL"
  - "BR-WSP-VERIFICATION-LINK-EXPIRY"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-VERIFICATION-LINK"
provenance:
  source: ".private/research/help-center/articles/15867463701394.md, .private/research/help-center/articles/360017645240.md, .private/research/help-center/articles/360014997360.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The person signs in with a different address, and the company's records show it.

## Trigger

The person wants to use another email address (for example a work address instead of a personal one).

## Preconditions

- The person is signed in to their personal profile.

## Main Flow

1. The person opens their account settings and enters the new address.
2. The product sends a verification link to the new address.
3. The person confirms through the link.
4. The product switches the sign-in address and updates the connected crew-member record.

## Alternative Flows

- None.

## Failure Conditions

- The address is in use: the change is refused.

## Postconditions

- The profile signs in with the new address.
