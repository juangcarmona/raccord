---
id: SB-WSP-RESTORE-LOSES-LATER
type: structured-behaviour
title: "Restore loses later work"
status: draft
illustrates:
  - "BR-WSP-RESTORE-DISCARDS-LATER"
given:
  - "A failed import on Tuesday damaged equipment data"
  - "Users also created projects on Tuesday"
when: "The installation is restored to the backup of Monday night after approval"
then:
  - "Tuesday's new projects and edits no longer exist"
uses-terms:
  - "TERM-WORKSPACE-BACKUP"
provenance:
  source: ".private/research/help-center/articles/360015902019.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Restore is all-or-nothing.

## Boundaries

None.
