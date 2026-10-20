---
id: UC-CON-FILTER-CONTACT-DIRECTORY
type: use-case
title: "Filter the contact directory"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-CON-TAG-FILTER-MODES"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-TAG"
  - "TERM-CONTACT-FOLDER"
---

## Goal

Find contacts by their tag assignments or folder placement.

## Trigger

The actor wants to narrow the contact directory to a subset of records.

## Preconditions

The actor may view contacts in the active workspace.

## Main Flow

1. The actor selects a folder, one or more tags, or both.
2. When multiple tags are selected, the actor chooses whether a contact must match any or all selected tags.
3. The product shows matching contacts from the active workspace.

## Alternative Flows

- The actor clears the filters to return to the unfiltered directory.

## Failure Conditions

- Filters do not reveal contacts from another workspace.

## Postconditions

The visible contact list reflects the selected folder and tag filters.
