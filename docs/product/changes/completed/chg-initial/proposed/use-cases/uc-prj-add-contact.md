---
id: UC-PRJ-ADD-CONTACT
type: use-case
title: "Add a contact"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PARTIES"
governed-by:
  - "BR-PRJ-CONTACT-NUMBER-ASSIGNMENT"
  - "BR-PRJ-ADDRESS-ROLES"
  - "BR-PRJ-TRAVEL-ESTIMATE-REFRESH"
  - "BR-PRJ-CONTACT-ROLES-BY-TAG"
  - "BR-PRJ-ONE-CONTACT-PER-AREA"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-TYPE"
  - "TERM-VISITING-ADDRESS"
  - "TERM-MAILING-ADDRESS"
  - "TERM-BILLING-ADDRESS"
  - "TERM-TRAVEL-ESTIMATE"
provenance:
  source: ".private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/29017857097618.md, .private/research/help-center/articles/360013628759.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A customer, venue or supplier is available in the contact database.

## Trigger

A new party appears.

## Preconditions

None.

## Main Flow

1. The user chooses company or private individual; for a company the user may look it up by name and adopt its details.
2. The user enters folder, number (or leaves it for automatic numbering), addresses, warning, automatic project note, contact persons and commercial defaults.
3. The user refreshes the travel estimate.
4. The product saves the contact.

## Alternative Flows

- Created inline while choosing a project's client or location.
- Venue with several stages: one contact per stage.

## Failure Conditions

- A contact with missing required details or an invalid address is refused with the reason and nothing is saved.
- A number already used by another contact is refused; leaving it empty assigns the next free number (BR-PRJ-CONTACT-NUMBER-ASSIGNMENT).
- If the distance service cannot be reached, the travel estimate fails visibly and can be refreshed later; no estimate is used until then (BR-PRJ-TRAVEL-ESTIMATE-REFRESH).
- An actor without permission to create contacts is refused and nothing changes.

## Postconditions

The contact can be chosen on projects.
