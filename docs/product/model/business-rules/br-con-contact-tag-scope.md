---
id: BR-CON-CONTACT-TAG-SCOPE
type: business-rule
title: Contact tags classify contacts only
status: active
applies-to: []
uses-terms:
  - TERM-CONTACT-TAG
---

## Rule

A contact tag may classify contacts in the active installation's Contacts store only. It cannot classify another record type. Management uses the existing Contacts access boundary. No client-selected workspace identifier or tenant column is introduced; separate deployments and databases isolate organizations.

## Rationale

Contacts classification stays within its accepted product boundary and access scope.

## Examples

A `venue` tag can classify venue contacts but is not available on equipment or crew records.

## Exceptions

None.
