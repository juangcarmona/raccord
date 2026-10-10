---
id: UC-RESTORE-RACCORD
type: use-case
title: "Restore an installation"
status: draft
primary-actor: "ACT-INSTALLATION-ADMINISTRATOR"
supporting-actors: []
governed-by: []
uses-terms: []
---

## Goal

An installation is rebuilt from a backup with every issued invoice unaltered.

## Trigger

Disaster recovery or a restore rehearsal.

## Preconditions

A backup set with valid checksums.

## Main Flow

1. The administrator restores the backup into an installation.
2. The tooling verifies checksums, restores records and documents and restarts the application.

## Alternative Flows

Restore rehearsal into a separate, empty installation.

## Failure Conditions

Checksum mismatch stops the restore before changing anything.

## Postconditions

Issued invoices show the same fingerprints and documents as before; issued records are still protected against change.
