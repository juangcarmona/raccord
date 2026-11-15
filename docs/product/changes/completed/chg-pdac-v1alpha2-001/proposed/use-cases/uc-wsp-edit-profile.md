---
id: UC-WSP-EDIT-PROFILE
type: use-case
title: "Maintain one's personal profile"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-PROFILE-OWNED-BY-PERSON"
  - "BR-WSP-QUALIFICATIONS-PER-COUNTRY"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-PROFILE-SECTION"
provenance:
  source: ".private/research/help-center/articles/38409981946770.md, .private/research/help-center/articles/15742354455698.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The person's professional information is complete and current.

## Trigger

The person wants to add or update personal details, skills, qualifications, travel data or documents.

## Preconditions

- The person is signed in on the web.

## Main Flow

1. The person opens their profile from their account settings.
2. The person opens a section, changes its content and saves that section.
3. The product stores the section; the company sees it according to its role permissions.

## Alternative Flows

- The person uploads a qualification document for a specific country.

## Failure Conditions

- The person is not signed in: the change is refused.
- A section has missing or invalid content, or a qualification has no country: it is refused with the reason and that section is not saved (BR-WSP-QUALIFICATIONS-PER-COUNTRY).
- An uploaded document is not accepted or cannot be stored: the upload fails visibly and can be retried.
- The profile was changed elsewhere in the meantime: the save is refused and the person must reload and retry.

## Postconditions

- The profile reflects the update.
