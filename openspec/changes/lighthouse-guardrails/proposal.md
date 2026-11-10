<!-- pdac-scope: none reason="Repository agent harness and verification tooling only; no product behavior changes." -->

# Adopt Lighthouse repository guardrails

## Why

Raccord needs a single, reproducible repository baseline and a Claude Code guard that prevents agent commands from bypassing local Git hooks.

## What Changes

- Initialize Lighthouse for the existing .NET and React TypeScript application, with its visual-theme profile and a pinned root tooling lockfile.
- Install and compile the default triage and refine workflow projections without enabling repository automation.
- Remove retired agent tooling and its copied skills without changing application behavior.
- Use Lighthouse's generated commit-message and pre-push hooks; retain Raccord's pre-commit checks and CI jobs.
- Reject Claude Code Bash calls that bypass Git hooks, and run the guard tests in CI.

## Impact

Only repository tooling, documentation, and CI configuration change. The existing .NET analyzers, architecture tests, Semgrep rules, and application code remain in place.
