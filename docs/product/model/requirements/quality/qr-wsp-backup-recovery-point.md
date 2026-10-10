---
id: QR-WSP-BACKUP-RECOVERY-POINT
type: quality-requirement
title: "Daily recovery point with configurable retention"
status: draft
quality-attribute: "recoverability"
applies-to:
  - "UC-WSP-REQUEST-RESTORE"
verification:
  - scenario-ref: "SB-WSP-RESTORE-LOSES-LATER"
  - scenario: "With the default schedule a backup exists for every night of the retention period"
uses-terms:
  - "TERM-WORKSPACE-BACKUP"
provenance:
  source: ".private/research/help-center/articles/360015902019.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The installation MUST have an automatic backup at least every 24 hours by default, on a schedule the company can change, and each backup MUST remain available for the configured retention period.

## Measurement

Inspect backup listings: with the default schedule at most 24 hours between consecutive backups; the oldest available backup is as old as the retention period; data loss on restore bounded by the time since the last backup.
