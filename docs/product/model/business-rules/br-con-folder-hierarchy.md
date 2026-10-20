---
id: BR-CON-FOLDER-HIERARCHY
type: business-rule
title: "Contact folders form a workspace-specific hierarchy"
status: active
applies-to:
    - "UC-CON-MANAGE-CONTACT-FOLDERS"
uses-terms:
    - "TERM-CONTACT-FOLDER"
---

## Rule

Each workspace has its own Contacts folder hierarchy. A contact may be unfiled or belong to at most one folder. Contacts folder assignments and folder filters do not apply to records of another type.

## Rationale

Contact organization remains separate between workspaces and module types.

## Examples

A folder called `Venues` in one workspace does not create or reveal that folder in another workspace.

## Exceptions

None.
