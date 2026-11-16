---
id: BR-CON-FOLDER-HIERARCHY
type: business-rule
title: Contact folders form a workspace-specific hierarchy
status: active
applies-to: []
uses-terms:
  - TERM-CONTACT-FOLDER
---

## Rule

Each installation's Contacts store has its own folder hierarchy. A contact may be unfiled or belong to at most one folder. Contacts folder assignments and folder filters do not apply to records of another type.

## Rationale

Contact organization remains separate between installations and module types.

## Examples

A Venues folder in one installation does not create or reveal a folder in another installation; organization isolation uses separate deployments and databases.

## Exceptions

None.
