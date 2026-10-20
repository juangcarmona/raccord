---
id: UC-CON-MANAGE-CONTACT-FOLDERS
type: use-case
title: "Manage contact folders"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-CON-FOLDER-HIERARCHY"
  - "BR-CON-FOLDER-DELETE-NONEMPTY"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-FOLDER"
---

## Goal

Organize contacts in a Contacts-specific folder hierarchy.

## Trigger

The actor needs to organize or reorganize the contact directory.

## Preconditions

The actor may manage the Contacts directory in the active workspace.

## Main Flow

1. The actor creates a root folder or subfolder, renames a folder, or reorders or moves it in the hierarchy.
2. The actor may assign a contact to one folder or remove its folder assignment.
3. The product saves the updated hierarchy and contact placement.

## Alternative Flows

- An empty folder may be deleted.

## Failure Conditions

- A folder containing a contact or child folder cannot be deleted.
- A folder cannot be moved under itself or one of its descendants.
- An actor without Contacts access is refused and nothing changes.

## Postconditions

The contact folder structure and contact placements are saved within the active workspace.
