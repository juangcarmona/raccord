---
id: UC-WSP-EDIT-COMPANY-DETAILS
type: use-case
title: "Maintain company details"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by: []
uses-terms:
  - "TERM-COMPANY-DETAILS"
  - "TERM-WORKSPACE-IMAGE"
provenance:
  source: ".private/research/help-center/articles/15426355918226.md, .private/research/help-center/articles/15744586750866.md, .private/research/help-center/articles/35600301773970.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The workspace presents the company correctly on documents and customer-facing pages.

## Trigger

Onboarding or a change of company data.

## Preconditions

- The administrator has configuration access.

## Main Flow

1. The administrator opens the company details.
2. The administrator enters name, address, contact data and logo.
3. The administrator optionally uploads a workspace image for sign-in, signing and payment pages.
4. The administrator saves.

## Alternative Flows

- None.

## Failure Conditions

- The user lacks configuration access: the changes are refused and the details are unchanged.
- A required detail is missing or invalid, or an uploaded logo or image is not an accepted image: it is refused with the reason and nothing is saved.
- Another administrator changed the details in the meantime: the save is refused and the user must reload and retry.

## Postconditions

- Company data and image are used in documents and on customer-facing pages.
