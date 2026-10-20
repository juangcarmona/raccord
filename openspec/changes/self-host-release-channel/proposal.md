<!-- pdac-scope: cited -->

## Why

Operators currently build the application from a source checkout and cannot select a supported release. A company that relies on Raccord needs a repeatable way to install a named version and recover if its database migration or new application fails.

## What Changes

- Define Docker Compose with versioned official application images as the supported self-host path.
- Pin releases by immutable version and digest, and distinguish official images from source or modified builds.
- Require verified database and document backups before migration; document restore as the recovery path when rollback is unsafe.
- Keep .NET Aspire for development and Azure as a separately documented deployment topology.
- Implement the agreed release and update path with repeatable clean-install, upgrade, migration-failure and restore checks.

## Capabilities

### New Capabilities

- `self-hosted-release-channel`: operators can choose a supported release and follow a documented recovery path.

### Modified Capabilities

None. The application behavior and data model do not change.

## Product definition impact

This change protects existing invoice, audit and document records during operations; it adds no product capability.

<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
