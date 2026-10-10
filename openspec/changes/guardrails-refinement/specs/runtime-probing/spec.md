# runtime-probing Specification

## Purpose

Probes the assembled runtime rather than the diff: after every merge to main, the Compose
stack boots for real and the OWASP ZAP baseline scan walks its HTTP surface. Placed
post-merge on purpose — its findings are about the deployed system, not a particular
change, and the article's tiering puts runtime probing after merge and before production
sign-off.

## ADDED Requirements

### Requirement: ZAP baseline scan runs against the booted Compose stack after merge

A post-merge workflow SHALL build the container image, boot the Compose stack (fresh
Postgres volume, migrations applied, healthy web service) and run the ZAP baseline scan
against the stack's published port, blocking on a FAILED scan, with the stack torn down
in every outcome.

#### Scenario: merged code ships a passive vulnerability or misconfiguration

- **WHEN** the ZAP baseline scan reports a finding ZAP scores as FAIL
- **THEN** the post-merge workflow fails

#### Scenario: the stack fails to boot

- **WHEN** `docker compose up --wait` times out or the web service never reports healthy
- **THEN** the workflow fails before ZAP runs, and the stack is still torn down

### Requirement: operational hygiene beyond pipelines is tracked

The human-side operational checks (backup drill, secret rotation, access review,
runbook currency, ZAP WARN triage) SHALL be recorded in
`docs/operations/ops-checklist.md` with an explicit "done" definition per item, and the
backup-restore drill SHALL have a seeded backlog issue.

#### Scenario: a checklist item is verified

- **WHEN** an ops checklist cycle runs
- **THEN** each checked item satisfies its documented done-definition (e.g. the restore
  drill executed end-to-end, not read)
