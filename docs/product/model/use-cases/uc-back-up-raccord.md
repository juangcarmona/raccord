---
id: UC-BACK-UP-RACCORD
type: use-case
title: "Back up an installation"
status: draft
primary-actor: "ACT-INSTALLATION-ADMINISTRATOR"
supporting-actors: []
governed-by: []
uses-terms: []
---

## Goal

A verifiable copy of all records and documents exists outside the installation.

## Trigger

Scheduled or before an upgrade.

## Preconditions

The installation is running.

## Main Flow

1. The administrator configures a backup schedule (for example nightly) and how many backup sets to keep, or runs a backup on demand.
2. The product's tooling exports the database and the stored documents with checksums at the scheduled times and removes sets beyond the retention.

## Alternative Flows

None.

## Failure Conditions

A failed export produces no backup set.

## Postconditions

A backup set exists.
