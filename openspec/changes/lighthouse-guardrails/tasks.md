<!-- pdac-scope: none reason="Repository agent harness and verification tooling only; no product behavior changes." -->

# Tasks

- [x] Inventory existing guardrails and select Lighthouse profiles for the actual application.
- [x] Initialize Lighthouse through its CLI and pin its verification dependency.
- [x] Install and compile the default SDLC workflows without enabling automation.
- [x] Remove retired agent tooling references and copied skills.
- [x] Add and test the Claude Code PreToolUse guard; run it in CI.
- [x] Preserve Raccord's application analyzers, architecture tests, Semgrep rules, pre-commit hook, and CI jobs.
- [x] Run the Release build, non-E2E tests, Lighthouse verifier, guard tests, and documentation lint.
- [x] Patch the existing arc42 snapshot DOMPurify pin found by the PR security gate and rerun its tests.
- [x] Update vulnerable frontend transitive locks and rerun lint, tests, and the high-severity dependency audit.
