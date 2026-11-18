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
- Patch the arc42 snapshot tool's DOMPurify pin and the frontend's transitive `undici` and `zod` locks after PR security and dependency-audit gates exposed vulnerable versions.

- Upgrade the CLI to 0.13.25 and SDLC to 0.29.0, then reinstall the existing epic, completion and Product Change workers in package mode and regenerate their locks.
- Retain the previously qualified implementation review profile and all current automation, audit and approval controls while retrying #294 separately.

- Configure the protected, source-independent `lighthouse/evidence` branch for sanitized native proof publication, preserving all existing required checks and approvals.
- Make the existing CI workflow dispatchable and subscribe the completion router to its actual name.

- Synchronize the existing Contacts browser filter assertions with the rendered server response after CI exposed a premature DOM snapshot.

- Enforce the owner-approved, expiring frontend development dependency exception with a tested evaluator in both CI and native independent checks, retaining the raw audit and rejecting every out-of-scope high/critical finding.

## Lighthouse 0.13.27 / SDLC 0.30.1 refresh

Update the exact npm pins, generated package projections, installed package-mode workflows and all nine compiled worker locks. Preserve the qualified implementation model profile, current checks, bounded security exception and automation settings. Assess unattended eligibility separately; this upgrade installs no new route and delegates no issue implementation or merge.

## Impact

Only repository tooling, documentation, and CI configuration change. The existing .NET analyzers, architecture tests, Semgrep rules, and application code remain in place.
