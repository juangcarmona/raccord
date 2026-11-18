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

The repository SHALL pin CLI 0.13.24 and SDLC 0.28.1 in its tooling manifest and lockfile, and all installed stage and maintenance workflows SHALL execute SDLC 0.28.1 with corresponding regenerated locks. Existing audit, approval and automation controls SHALL be preserved.

#### Scenario: Upgrade verification

- **WHEN** the published packages are upgraded
- **THEN** the CLI verifier passes and every installed worker source and compiled lock references the selected SDLC version

#### Scenario: Native retry

- **WHEN** issue #294 is retried through the upgraded router
- **THEN** its actual execution receipt identifies the upgraded harness, and prior failed proof is retained without relabelling
