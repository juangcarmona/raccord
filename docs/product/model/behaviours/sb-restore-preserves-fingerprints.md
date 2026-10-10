---
id: SB-RESTORE-PRESERVES-FINGERPRINTS
type: structured-behaviour
title: "A restore preserves issued invoices"
status: draft
illustrates:
  - "UC-RESTORE-RACCORD"
given:
  - "A backup was taken after invoice F2026-00001 was issued"
when: "The backup is restored into a new, empty installation"
then:
  - "F2026-00001 shows the same fingerprint and an identical document"
  - "Changing or deleting audit events in the restored installation is still refused"
uses-terms: []
---

## Intent

Backups keep both the data and its protection.

## Boundaries

None.
