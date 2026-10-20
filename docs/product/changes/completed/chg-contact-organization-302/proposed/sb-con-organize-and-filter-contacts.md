---
id: SB-CON-ORGANIZE-AND-FILTER-CONTACTS
type: structured-behaviour
title: "Organize and filter contacts with tags and folders"
status: active
illustrates:
  - "UC-CON-TAG-CONTACTS"
  - "UC-CON-MANAGE-CONTACT-FOLDERS"
  - "UC-CON-FILTER-CONTACT-DIRECTORY"
given:
  - "An authorized coordinator has two synthetic contacts in the active workspace"
when: "The coordinator creates a tag and folder, assigns the tag and one contact to that folder, then filters by the tag and folder"
then:
  - "The filtered directory shows the contact with both the selected tag and folder"
  - "The other contact is excluded from the filtered results"
  - "A nonempty folder cannot be deleted"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-TAG"
  - "TERM-CONTACT-FOLDER"
---

## Intent

Shows how tags and folders organize synthetic Contacts records and how those filters narrow the directory.

## Boundaries

Only Contacts in the active workspace are considered; cross-workspace isolation is separately demonstrated by SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE.
