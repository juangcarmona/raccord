---
id: FR-CON-005
type: functional-requirement
title: "Organize contacts in folders"
status: active
derived-from:
  - "UC-CON-MANAGE-CONTACT-FOLDERS"
  - "BR-CON-FOLDER-HIERARCHY"
  - "BR-CON-FOLDER-DELETE-NONEMPTY"
verification:
  - scenario-ref: "SB-CON-ORGANIZE-AND-FILTER-CONTACTS"
  - scenario: "A contact is assigned to a folder and appears in its filtered view"
  - scenario: "A folder containing a contact or subfolder cannot be deleted"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-FOLDER"
---

## Requirement

The product MUST let authorized users create and manage a Contacts-specific hierarchy of root folders and subfolders, and assign each contact to at most one folder. A contact MAY remain unfiled. The product MUST refuse deletion of a folder containing a contact or child folder and MUST prevent a folder from being moved into itself or a descendant.

## Rationale

Users can organize contacts into a hierarchy without making folder setup mandatory or risking recursive folder structures.
