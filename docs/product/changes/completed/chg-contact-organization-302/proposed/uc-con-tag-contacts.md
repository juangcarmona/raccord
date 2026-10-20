---
id: UC-CON-TAG-CONTACTS
type: use-case
title: "Assign tags to contacts"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-CON-CONTACT-TAG-SCOPE"
  - "BR-CON-TAG-FILTER-MODES"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-TAG"
---

## Goal

Group contacts with tags and find them again through the directory.

## Trigger

The actor needs to classify contacts or narrow the directory to a group.

## Preconditions

The actor may manage contacts in the active workspace.

## Main Flow

1. The actor creates a contact tag or chooses an existing Contacts tag.
2. The actor assigns or removes the tag on a contact.
3. The actor filters the directory by one or more tags and chooses whether results match any or all selected tags.

## Alternative Flows

- The actor removes a tag from a contact; the contact remains in the directory.

## Failure Conditions

- A tag containing spaces or an actor without Contacts access is refused and no unauthorized change occurs.
- A tag created for Contacts is unavailable to other record types.

## Postconditions

The contact's tag assignments are saved and the directory filter returns only contacts in the active workspace.
