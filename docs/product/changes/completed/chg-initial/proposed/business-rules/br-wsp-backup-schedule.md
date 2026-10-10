---
id: BR-WSP-BACKUP-SCHEDULE
type: business-rule
title: "Backups are taken on a schedule and kept for a retention period"
status: draft
applies-to:
  - "UC-WSP-REQUEST-RESTORE"
uses-terms:
  - "TERM-WORKSPACE-BACKUP"
provenance:
  source: ".private/research/help-center/articles/360015902019.md, .private/research/help-center/articles/36084108749842.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A backup of the installation is taken automatically on the schedule the company configures (every night by default, at a quiet hour) and each backup is kept for the configured retention period.

## Rationale

There is always a recent version to return to.

## Examples

- Running a large import in the morning means the previous night's backup is a safe fallback.

## Exceptions

None.
