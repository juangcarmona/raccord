<!-- pdac-scope: none reason="Repository engineering tooling and CI process; implements no product behaviour defined in docs/product/model." -->

## Purpose

Local git hooks run the repo's commit-time gates without manual setup, so the cage is active on
the first commit after a fresh clone — but never block CI or an explicit opt-out.

## ADDED Requirements

### Requirement: Hooks install automatically on build

The repository SHALL ship a HuskyInstall MSBuild target in the root Directory.Build.props that configures `git config core.hooksPath .husky` before every build.

#### Scenario: Fresh clone

- **WHEN** a developer clones the repository and runs `dotnet restore && dotnet build`
- **THEN** `git config core.hooksPath` prints `.husky` with no manual step performed

#### Scenario: CI is exempt

- **WHEN** a build runs on GitHub Actions (CI or GITHUB_ACTIONS set)
- **THEN** the target does not touch git configuration

### Requirement: Hooks can be explicitly bypassed

Hooks SHALL check `HUSKY=0` and exit successfully without running any check.

#### Scenario: Opt-out

- **WHEN** a developer commits with `HUSKY=0 git commit ...`
- **THEN** no pre-commit or commit-msg check runs and the commit succeeds

### Requirement: Hooks operate repo-wide from any worktree

The commit-msg hook SHALL resolve the commit-message file against the repository root, accepting both POSIX and Windows drive-letter absolute paths as well as paths relative to the worktree root.

#### Scenario: Linked worktree on Windows

- **WHEN** a commit is made from a linked worktree whose git passes a `C:\...` path to the hook
- **THEN** commitlint still receives the correct file and validates it
