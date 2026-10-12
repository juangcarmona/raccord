---
name: guardrails
description: Raccord adds auto-installed commit-time hooks mirrored in CI, test conventions, result-checking security and license gates, nightly mutation testing, post-merge runtime probing and an OpenSpec spec layer.
status: Accepted
date: 2026-10-10
deciders: ["Juan G. Carmona"]
tags: ["guardrails", "ci", "security", "testing", "conventions", "spec"]
---

# ADR-0016: Add commit-time hooks, CI mirrors, security gates and spec tooling as guardrails

## Context

ADR-0015 placed every quality gate server-side: CI alone verified formatting, lint,
commit hygiene and dependencies. A local `--no-verify` or a stale hook install silently
skipped them until PR time, test naming had no machine-checked convention, and the
security posture (secrets, insecure patterns, license compliance) had no result-checking
gates at all. The guiding principle, following the "we don't review code anymore" school
of guardrails: the cage, not line-reading — hard, automated gates that check results,
and each gate has to earn its place by catching something real.

## Decision

We will add the following guardrails on top of ADR-0015's build and quality gates.

### Commit-time gates (auto-installed, with an escape)

- Husky is a .NET local tool (root `dotnet-tools.json`), activated by a HuskyInstall
  MSBuild target in the root `Directory.Build.props` (`git config core.hooksPath .husky`)
  on first build — skipped on CI and with `HUSKY=0`. Documented escape:
  `HUSKY=0 git commit ...`.
- **Pre-commit**: CSharpier on staged `.cs` files (re-staged after formatting), then
  lint-staged from the frontend (ESLint `--fix` + Prettier on staged TS/TSX and
  formatted text files).
- **Commit-msg**: commitlint enforcing conventional commits with a restricted type-enum
  (feat, fix, chore, docs, refactor, test, ci).
- **Formatting split**: CSharpier replaces `dotnet format` for C# — one formatter per
  language, not two that disagree. Style rules stay enforced by
  `EnforceCodeStyleInBuild` + `TreatWarningsAsErrors` (ADR-0015). Prettier covers the
  frontend.

### CI mirrors (bypasses still get caught)

The `lint` job re-runs every commit-time gate in check mode (CSharpier `check`,
Prettier `--check`, commitlint over the PR commit range base..head with `fetch-depth: 0`),
so a `HUSKY=0` or `--no-verify` bypass still produces a red build before merge.
ESLint is deliberately not duplicated — the frontend job already fails on it.

### Test conventions

- Assertions use Shouldly across every test project.
- Test names follow `Thing_Should_Outcome`, enforced by reflection over **all** test
  assemblies (`TestNaming_Should_Constraint` in Raccord.Architecture.Tests), including
  E2E journeys.

### E2E (ADR-0017)

A new `Raccord.EndToEndTests` project drives the real SPA with Playwright over the
booted Aspire AppHost — see ADR-0017 for the strategy.

### Security gates (all result-checking, all blocking on PR)

- **TruffleHog** over git history since the PR base (`fetch-depth: 500`) — secrets.
- **Semgrep** with in-repo rules (`.semgrep/`) plus `p/owasp-top-ten` and `p/secrets`:
  no `HttpClient` or direct file IO in Domain/Application (the static twin of the
  ForbiddenInCore arch rule), no hardcoded credential-looking literals. Each rule was
  verified to fire on a planted violation.
- **Trivy** fs scan (high/critical) plus an image scan in the sbom job.
- The deeper per-ecosystem audits stay: `dotnet list package --vulnerable --include-transitive`,
  `npm audit --audit-level=high`.

### SBOM and license policy (load-bearing for ADR-0002)

The `sbom` job builds the image, Trivy-scans it, publishes the Syft SPDX SBOM
(`sbom.spdx.json`) as an artifact, and enforces the license allowlist via
`tools/dependency-licenses`, which reads the restored NuGet graph (nuspec license
expressions) and `package-lock.json`. NuGet licenses live in nuspec metadata, not in the
compiled DLLs, so an image-only SBOM cannot carry them — the policy check reads the
graph. A copy-left or non-distributable surprise (e.g. an accidental GPL dependency)
must fail the build before it ships in a deliverable. Allowlist: MIT, Apache-2.0,
BSD-2/3-Clause, ISC, BlueOak-1.0.0, PostgreSQL, Python-2.0, MPL-2.0, CC0-1.0, CC-BY-4.0,
AGPL-3.0 (Raccord itself). Packages predating license expressions are explicitly mapped
after reading their license, never passed by silence.

### Mutation testing (nightly, not per-PR)

`dotnet-stryker` (root tool manifest) runs nightly over the Domain tests with
thresholds high 80 / low 70 / break 60 and publishes the HTML report. Deliberately
nightly: with the current suite density the score is as much a test-gap inventory as a
grade, and a flaky per-PR gate trains people to ignore it. When the suite sustains a
stable score, this moves per-PR. First baseline (2026-10-10, 320 tested mutants):
**67.81 %** — 217 killed, 103 survived; the surviving-mutant inventory is the first
worklist for tightening the Domain suite.

Configuration note (xunit.v3, learned the hard way): the tests run on xunit.v3, whose
per-mutant test outcomes don't propagate through Stryker's default VsTest runner — every
mutant reports Survived and the score reads a meaningless 0.00 %. The config pins
`"test-runner": "mtp"` (the runner xunit.v3 was built for), and
`"coverage-analysis": "off"` — the MTP runner's coverage data is aggregate-only, which
would run every mutant against the whole suite anyway.

### Post-merge runtime probing

A separate `post-merge` workflow (push to main) boots the Compose stack (`--wait`,
fresh volumes) and runs the OWASP ZAP baseline scan against it, blocking on FAIL.
Post-merge on purpose: its findings are about the assembled runtime, not a change.
Operational hygiene that a pipeline can't verify (backup drills, secret rotation,
access review, runbook currency, ZAP WARN triage) lives in
`docs/operations/ops-checklist.md`, seeded by the backup-restore drill issue (#5).

### Spec layer (OpenSpec + PDaC boundary)

OpenSpec (`openspec/`) is the behavioral spec layer: each change ships
`proposal.md`, `tasks.md`, and per-capability delta specs, validated in CI. PDaC
(`docs/product/`) stays the product-model source of truth — OpenSpec specs describe
implementation behavior per change; product decisions go through Product Changes, not
specs.

## Consequences

- **Positive:**
    - A `HUSKY=0` or `--no-verify` bypass still produces a red build before merge.
    - Each gate included here was verified to catch a planted violation before being
      merged; per ADR-0015's principle, a gate that stops catching gets removed rather than
      muted.
- **Negative:**
    - Direct pushes to main skip the commitlint mirror (it runs on `pull_request` only);
      branch protection is the policy answer.
    - Hookless machines (no node on PATH) get a failing pre-commit — the documented
      escape is `HUSKY=0`.
- **Neutral:**
    - Mutation testing moves per-PR when the suite sustains a stable score.

## Alternatives considered

- **Keep every gate server-side only (ADR-0015 as it was):** rejected because a local `--no-verify` or a stale hook install silently skipped the gates until PR time.
- **Keep `dotnet format` alongside CSharpier:** rejected — one formatter per language, not two that disagree.
- **Duplicate ESLint in the `lint` job:** rejected because the frontend job already fails on it.
- **Enforce the license policy from an image-only SBOM:** rejected because NuGet licenses live in nuspec metadata, not in the compiled DLLs, so an image-only SBOM cannot carry them.
- **Mutation testing per PR:** rejected for now because the score is as much a test-gap inventory as a grade, and a flaky per-PR gate trains people to ignore it.
- **Stryker's default VsTest runner:** rejected because xunit.v3 per-mutant outcomes don't propagate through it, so every mutant reports Survived.

## References

- Extends: ADR-0015 (build and quality gates)
- Related: ADR-0002, ADR-0017
- Operations checklist: `docs/operations/ops-checklist.md`
- Issue #5: backup-restore drill
