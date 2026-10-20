---
id: BR-CON-FOLDER-DELETE-NONEMPTY
type: business-rule
title: "Contact folders with contents cannot be deleted"
status: active
applies-to:
    - "UC-CON-MANAGE-CONTACT-FOLDERS"
uses-terms:
    - "TERM-CONTACT-FOLDER"
---

## Rule

A contact folder containing a contact or child folder cannot be deleted. The actor must move its contacts and child folders elsewhere before deletion.

## Rationale

Refusing deletion preserves an explicit destination for every contact already organized in the folder hierarchy.

## Examples

A folder containing one contact cannot be deleted until that contact is moved or unfiled; a parent folder containing a subfolder cannot be deleted until the subfolder is moved.

## Exceptions

None.
