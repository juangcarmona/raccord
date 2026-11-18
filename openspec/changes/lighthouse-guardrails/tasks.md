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
- [x] Upgrade the Lighthouse harness and SDLC package projection; compile triage, refine, propose, implement, and apply-review workflows.
- [x] Subscribe CI to App-created verification refs and keep automatic implementation disabled.
- [x] Apply the approved Product Change `CHG-PDAC-V1ALPHA2-001`, renew OpenSpec citations, and verify the Lighthouse foundation.

- [x] Pin CLI 0.13.24 and SDLC 0.28.1, apply the reviewed CLI upgrade and reinstall the existing worker selections.
- [x] Regenerate all nine worker locks with gh-aw v0.93.2 and preserve the qualified implementation profile.
- [x] Verify package/workflow pin agreement, Lighthouse, strict OpenSpec, citations and documentation checks.

- [x] Upgrade the reviewed delivery branch to CLI 0.13.25 and SDLC 0.29.0 and regenerate every installed worker projection.
- [x] Recheck foundation, strict OpenSpec, citations, Markdown and exact installed workflow versions for the new release.

- [x] Reconcile native 44239806576: preserve its blocked result and 13 passed independent commands, with no runtime PR created.
- [x] Create source-independent protected evidence storage and observe its actual ruleset revision without changing main protections.
- [x] Configure evidence publication, dispatchable full CI and router subscription; validate current policy and regenerated workflows.

- [x] Replace the premature Contacts table snapshot wait with a Playwright assertion bound to the completed API response, retaining the original filter assertions.
- [ ] Confirm the corrected Contacts journey and full selection in current-head CI.
