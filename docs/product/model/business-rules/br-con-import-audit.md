---
id: BR-CON-IMPORT-AUDIT
type: business-rule
title: Contact imports record an auditable outcome
status: active
applies-to: []
uses-terms: []
---

## Rule

Each confirmed contact import records the actor, timestamp, import identifier, file type and outcome counts, including created and rejected rows. The audit record does not retain uploaded bytes or raw contact values.

## Rationale

Workspace administrators can account for directory changes without duplicating contact data in the audit history.

## Examples

An import record shows that an administrator created 23 contacts and 2 rows were rejected at a given time, without copying the spreadsheet contents into the audit record.

## Exceptions

None.
