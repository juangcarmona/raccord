---
id: JRN-OPERATE-INSTALLATION
type: journey
title: "Operate a self-hosted installation"
status: draft
primary-actor: "ACT-INSTALLATION-ADMINISTRATOR"
steps:
  - use-case: "UC-INSTALL-RACCORD"
  - use-case: "UC-BACK-UP-RACCORD"
  - use-case: "UC-RESTORE-RACCORD"
---

## Intended Outcome

A company runs Raccord on its own infrastructure with recoverable, unaltered records.

## Entry Conditions

Infrastructure is available.

## Journey Narrative

The administrator installs Raccord, schedules backups and rehearses a restore into a clean installation to prove that issued invoices come back identical.

## Variants and Branches

Upgrades: back up, then apply the new version, whose schema changes run before the application starts.

## Completion Conditions

A rehearsed restore produced identical invoice fingerprints and documents.
