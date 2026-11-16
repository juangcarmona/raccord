---
id: SB-PRJ-IMPORT-FAILED-ROWS-FILE
type: structured-behaviour
title: Failed import rows are returned as a file
status: draft
illustrates:
  - BR-PRJ-IMPORT-RESULT-REPORT
given:
  - An import file has 50 rows, 3 with invalid values
when: The import runs
then:
  - 47 rows are imported
  - The result lists the 3 failed rows with reasons
  - A file with the 3 failed rows and reasons can be downloaded
uses-terms: []
provenance:
  source: .private/research/help-center/articles/360013553000.md, .private/research/help-center/articles/39225660180626.md, .private/research/help-center/articles/4413562661138.md
  confidence: high
  recovered-from: documentation
---

## Intent

Partial imports are recoverable.

## Boundaries

This example applies to import profiles that provide downloadable failed-row files, including equipment imports and the extended Contacts profile. It does not require the excluded behavior in UC-CON-IMPORT-CONTACTS.
