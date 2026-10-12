<!-- pdac-scope: none reason="Repository engineering tooling and CI process; implements no product behaviour defined in docs/product/model." -->

## Purpose

Every check the commit-time hooks perform is re-run by CI in check mode, so a local hook bypass (`--no-verify`, `HUSKY=0`) is still caught before merge — the hooks and the mirror are two detections of one rule, not two rules.

## ADDED Requirements

### Requirement: CI re-runs every commit-time gate in check mode

The CI lint job SHALL run, in check mode, the same formatters and linters the hooks apply: `dotnet csharpier check .` over the repository, `prettier --check` over the frontend, and commitlint over every commit in the PR range.

#### Scenario: Hook bypass with misformatted code

- **WHEN** a developer commits misformatted `.cs` with `--no-verify` and opens a PR
- **THEN** the CI lint job fails on `csharpier check`

#### Scenario: Hook bypass with non-conventional message

- **WHEN** a PR contains a commit whose message is not conventional
- **THEN** the CI lint job fails on commitlint over the PR commit range

### Requirement: The mirror needs full PR history

The lint job SHALL check out the repository with enough history (fetch-depth 0) for commitlint to see every commit between the PR base and head.

#### Scenario: Long-lived branch

- **WHEN** a PR branch has dozens of commits since it forked from main
- **THEN** every one of them is validated by commitlint

### Requirement: Direct pushes to main are not commitlint-mirrored

The commitlint mirror SHALL run only on pull_request events; pushes to main are out of its scope, and branch protection is the policy answer for them.

#### Scenario: Direct push to main

- **WHEN** a non-conforming commit is pushed directly to main
- **THEN** the lint job does not run commitlint over it
