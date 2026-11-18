<!-- pdac-scope: none reason="Repository agent harness and verification tooling only; no product behavior changes." -->

# Design

Lighthouse owns its generated manifest projection, commit-message hook, pre-push hook, and verification workflow. Raccord keeps its existing analyzer and architecture-test enforcement; the Lighthouse .NET profile adds guidance without installing overlapping analyzer code. The React profile requires visual-theme, so both are selected for the existing browser application.

The pinned SDLC package installs its default triage and refine workflows through `install-workflows.mjs` in package mode. `gh aw v0.93.2` compiles their lockfiles. The router requires `LIGHTHOUSE_AUTOMATION_ENABLED=true` before dispatch, and that repository variable is not set by this change.

The Claude Code PreToolUse command invokes a dependency-free Node script. The script reads `tool_input.command` from stdin, exits 2 with a repair message for hook bypasses, and exits 0 for ordinary calls or malformed input. A shell wrapper also treats script launch failures as nonblocking. The guard does not change Git or Husky behavior for human users.

The repository already mirrors its prior commitlint policy in the CI lint job. Lighthouse's generated commit hook remains byte-for-byte managed so `lighthouse verify` can check it.

## Published package upgrade for issue #294

Pin CLI 0.13.25 and SDLC 0.29.0 in the repository lock and manifest. Apply the reviewed CLI upgrade and use the SDLC installer with the existing epic, completion, Product Change and maintenance selections; regenerate all nine worker locks with gh-aw v0.93.2. Preserve `LIGHTHOUSE_MODEL_PROFILE: plan` on the implementation worker from the last native recovery branch so its internal reviews use the configured profile. No new unattended or epic worker is installed and no repository variable changes.

The release adds retained visual assessment and separate PNG artifacts to planning and implementation publication. Check the actual copied workflow pins and compiled locks as well as the package metadata. A native #294 retry remains a separate execution with new receipts: the previous failed evidence and recovered source remain intact, and neither installation nor local verification establishes runtime completion. The existing full frontend dependency audit remains blocking.

## Evidence publication prerequisite

Native retry 44239806576 retained valid source and independent command results, but failed publication before creating its runtime PR because the source profile omitted `evidenceBranch` and `completion.publishEvidence`. Configure `lighthouse/evidence`, whose new root contains only an evidence README, and an additive active ruleset prohibiting deletion and non-fast-forward updates with no bypass actors. Record the owner-observed ruleset revision through the packaged read-only observer; existing main protection, required checks and approvals remain unchanged. Publish only bounded, sanitized reports and native visual captures.

Use the existing `ci.yml` for post-merge verification, add its manual dispatch event and regenerate the router subscriptions. Its classifier already selects the full application checks when no base event SHA exists, so a manual run cannot silently become a documentation-only check. These configuration changes do not qualify the security audit or turn the prior blocked envelope into a passed result. Integrate the reviewed policy before another native run reads main; preserve the prior candidate and receipts.

## Existing browser assertion timing

CI 44239553395 received the successful any-tag directory response but read the table before React rendered both expected contacts. The existing helper waited only for an already-visible first row. Wait for the ordered contact names from that exact response to appear in the table, then retain every any/all/folder positive and negative assertion. No application filter behavior or selected journey changes. The next required CI run validates the corrected journey and complete suite; security audit remains independently blocking.

## Owner-approved temporary development dependency exception

The owner explicitly accepted the scoped exception in conversation; comment6983388300 records that decision. `.github/frontend-audit-exception.json` binds GHSA-fc5q-39xc-vjpf/source1247710 to the reviewed frontend lock SHA and eight affected development nodes, expiring at 2026-11-24T20:45:00Z. `scripts/check-frontend-audit.mjs` still runs the full npm audit. It validates report structure/counts, traverses transitive advisory chains and fails on production nodes, other high/critical findings, unknown/cyclic chains, changed lock bytes, missing approval or expiry. It never accepts a critical finding.

CI retains the original full JSON as an artifact; native command output also retains it. Passing under the approved exception is explicitly reported instead of claiming a clean raw audit. Regression cases exercise expiry, altered graphs, production scope, hidden advisories, URL substitution and malformed reports. The same gate is added to default native independent checks, while the historical closed299 override and original blocked receipts are preserved. Remove the exception and restore the ordinary audit when a verified upstream fix is adopted.

## Lighthouse 0.13.27 / SDLC 0.30.1 refresh

Use the published exact packages and the supported foundation upgrade and workflow installer. Regenerate the existing epic, completion, maintenance and Product Change selections with gh-aw v0.93.2. The implementation Markdown retains its adopter-owned `LIGHTHOUSE_MODEL_PROFILE: plan` override before recompilation. Keep `.lighthouse/implement.json`, GitHub protection, security policy and repository variables byte-for-byte unchanged. No unattended workflow or policy is installed: source support in the SDK does not establish adopter readiness or execution permission.
