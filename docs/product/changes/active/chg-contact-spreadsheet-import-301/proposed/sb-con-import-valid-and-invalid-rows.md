---
id: SB-CON-IMPORT-VALID-AND-INVALID-ROWS
type: structured-behaviour
title: "Valid contact rows import while invalid rows are rejected"
status: active
illustrates:
  - "BR-CON-IMPORT-ROW-ATOMICITY"
given:
  - "A supported spreadsheet contains three valid contact rows and one row with an invalid required value"
when: "An authorized administrator reviews the mapping and confirms the import"
then:
  - "The three valid rows each create one complete contact"
  - "The invalid row creates no contact and is listed with its reason"
  - "The audit history records the actor, time and outcome counts without storing the file or contact values"
uses-terms:
  - "TERM-CONTACT"
---

## Intent

The user can complete a useful import while rejected rows remain recoverable and no partial contact is saved.

## Boundaries

This behavior covers new contact records only. It does not import contact persons, update existing records, assign folders or tags, or support other record types.
