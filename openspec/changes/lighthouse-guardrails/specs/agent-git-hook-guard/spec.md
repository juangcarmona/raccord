<!-- pdac-scope: none reason="Repository agent harness and verification tooling only; no product behavior changes." -->

# Agent Git hook guard

## ADDED Requirements

### Requirement: Claude Code must not bypass repository Git hooks

The repository SHALL reject Claude Code Bash calls that run `git commit` or `git push` with `--no-verify`, run `git commit` with `-n`, or set `HUSKY=0`.

#### Scenario: Bypass attempt

- **WHEN** a Bash call contains a Git hook bypass
- **THEN** the PreToolUse hook exits 2 and explains how to retry with hooks enabled

#### Scenario: Ordinary command

- **WHEN** a Bash call does not bypass Git hooks
- **THEN** the PreToolUse hook exits 0

#### Scenario: Guard failure

- **WHEN** the guard receives malformed input or cannot start
- **THEN** the PreToolUse hook allows the session to continue

### Requirement: Guard behavior must be verified in CI

CI SHALL run the Node test suite for blocked, allowed, and malformed Bash calls.

#### Scenario: Pull request verification

- **WHEN** CI runs for a pull request
- **THEN** the guard test suite is executed in the existing CI workflow

### Requirement: Released harness and installed workers must agree

The repository SHALL pin CLI 0.13.25 and SDLC 0.29.0 in its tooling manifest and lockfile, and all installed stage and maintenance workflows SHALL execute SDLC 0.29.0 with corresponding regenerated locks. Existing audit, approval and automation controls SHALL be preserved.

#### Scenario: Upgrade verification

- **WHEN** the published packages are upgraded
- **THEN** the CLI verifier passes and every installed worker source and compiled lock references the selected SDLC version

#### Scenario: Native retry

- **WHEN** issue #294 is retried through the upgraded router
- **THEN** its actual execution receipt identifies the upgraded harness, and prior failed proof is retained without relabelling

### Requirement: Native evidence publication must have protected storage

The repository SHALL retain sanitized native reports on a separate `lighthouse/evidence` branch with active deletion and non-fast-forward protection and no bypass actors. The reviewed profile SHALL declare publication and the existing dispatchable CI workflow without weakening source merge checks or approvals.

#### Scenario: Publication admission

- **WHEN** a native worker publishes its retained visual evidence
- **THEN** its source profile identifies the protected evidence branch and a current owner-observed ruleset revision

#### Scenario: Verification after merge

- **WHEN** completion requests `ci.yml` on the accepted main revision
- **THEN** all application CI jobs remain selected when the dispatch has no base SHA

### Requirement: Security exception must remain narrow and expire

CI and default native checks SHALL retain the full frontend audit and permit only owner-approved GHSA-fc5q-39xc-vjpf in the unchanged reviewed development graph before 2026-11-24T20:45:00Z. Every other high/critical finding, production affected node, changed graph, malformed report or missing/expired approval SHALL fail.

#### Scenario: Approved development finding

- **WHEN** the full audit reports only the approved high-severity development chain before expiry
- **THEN** the gate passes with explicit exception use and retains the unchanged raw findings

#### Scenario: Expired or wider finding

- **WHEN** the approval expires or the report contains an unapproved high/critical advisory or production node
- **THEN** the gate refuses integration
