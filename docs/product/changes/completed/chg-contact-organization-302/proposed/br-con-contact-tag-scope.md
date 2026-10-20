---
id: BR-CON-CONTACT-TAG-SCOPE
type: business-rule
title: "Contact tags classify contacts only"
status: active
applies-to:
  - "UC-CON-TAG-CONTACTS"
uses-terms:
  - "TERM-CONTACT-TAG"
---

## Rule

A contact tag may be assigned to one or more contacts in its workspace. It cannot be assigned to another record type. Managing a tag or assignment requires permission to manage contacts in that workspace.

## Rationale

Contacts classification stays within its accepted product boundary and access scope.

## Examples

A `venue` tag can classify venue contacts but is not available on equipment or crew records.

## Exceptions

None.
