<!-- pdac-scope: none reason="Repository engineering tooling and CI process; implements no product behaviour defined in docs/product/model." -->

## 1. Tooling

- [x] 1.1 Add husky 0.9.1 and csharpier 1.3.0 to the root dotnet-tools.json; add a
      HuskyInstall MSBuild target to the root Directory.Build.props that sets
      `git config core.hooksPath .husky` on BeforeBuild, skipped on CI and `HUSKY=0`. Verify: a
      fresh build leaves `git config core.hooksPath` printing `.husky`.
- [x] 1.2 One-time `dotnet csharpier format .`; swap the CI backend Format step for
      `dotnet tool restore` + `dotnet csharpier check .`. Verify: `dotnet csharpier check .`
      passes and the build is green with warnings-as-errors.
- [x] 1.3 Add prettier, lint-staged and @commitlint to the frontend devDependencies with
      `.prettierrc.json`, `.prettierignore` (ignores openapi/, the generated schema and the
      lockfile) and `.lintstagedrc.json`. Verify: `npm run format:check` passes.

## 2. Hooks

- [x] 2.1 `.husky/pre-commit`: HUSKY=0 escape; CSharpier + re-add for staged `*.cs`; then
      `npm exec lint-staged` from src/Raccord.Frontend. Verify: a staged misformatted `.cs`
      file is reformatted by committing.
- [x] 2.2 `.husky/commit-msg`: HUSKY=0 escape, Windows/worktree path normalization, commitlint
      with `.config/commitlint.config.cjs`. Verify: `update stuff` is rejected; a conventional
      message passes.

## 3. CI mirror

- [x] 3.1 New lint job in .github/workflows/ci.yml: checkout fetch-depth 0, CSharpier check,
      Prettier --check, commitlint over the PR base..head range. Verify: lint job green on the
      branch; a deliberately non-conventional commit message fails it.

## 4. Test conventions

- [x] 4.1 Shouldly 4.3.0 in Directory.Packages.props, referenced once from
      tests/Directory.Build.props with a global using; migrate all Assert call sites. Verify:
      `dotnet test Raccord.slnx` green with the same test count.
- [x] 4.2 Rename every test to Thing_Should_Outcome and add
      tests/Raccord.Architecture.Tests/TestNaming_Should_Constraint.cs reflecting over all
      Raccord.\*.Tests assemblies. Verify: a planted badly-named test fails the rule naming the
      offender.

## 5. Spec layer

- [x] 5.1 Author openspec/config.yaml (this change) and validate it in CI. Verify:
      `npx -y @fission-ai/openspec@1.12.0 validate --all --no-interactive` passes locally and
      in the openspec CI job.

## 6. E2E journeys

- [x] 6.1 tests/Raccord.EndToEndTests: csproj (Aspire.Hosting.Testing 13.6.0, Playwright
      1.63.0, xunit.v3), xunit.runner.json, Infrastructure/ (DistributedAppFixture,
      E2ECollection, E2ETraits, BrowserJourneyBase, PlaywrightArtifacts). Verify: the solution
      builds with warnings-as-errors.
- [x] 6.2 Four smoke journeys (boot, customer, billable line, invoice finalize) over the real
      SPA. Verify: `dotnet test tests/Raccord.EndToEndTests --filter "Quarantine!=true"` green
      with Docker running.
- [x] 6.3 per-pr-selection.txt + .github/scripts/check-e2e-selection.sh; e2e job in ci.yml
      (chromium --with-deps, manifest check, journey run, artifacts on failure); backend job
      excludes E2E. Verify: guard script prints "matches the manifest"; discovery equals the
      manifest.
- [x] 6.4 New test project wired into Raccord.slnx, Architecture.Tests' naming-rule references
      and tests/README.md.

## 7. Security gates

- [x] 7.1 New `security` job in .github/workflows/ci.yml: checkout with fetch-depth 500;
      TruffleHog over the ref's history since the PR base; Semgrep over `.semgrep/` +
      p/owasp-top-ten + p/secrets; Trivy fs + image scans, blocking on HIGH/CRITICAL.
      Verify: all three tools run clean on the branch (fs and image verified locally via
      the aquasec/trivy and returntocorp/semgrep containers).
- [x] 7.2 In-repo rules in .semgrep/csharp-core-rules.yml: no HttpClient in Domain/
      Application, no direct file IO in Domain/Application, no hardcoded credential-looking
      literals. Verify: planting each violation makes `semgrep --error` exit 1 (checked
      locally for all three; plants removed afterwards).

## 8. SBOM and license policy

- [x] 8.1 `sbom` job in .github/workflows/ci.yml: build the image, Trivy image scan
      (HIGH/CRITICAL blocking), anchore/sbom-action publishing sbom.spdx.json. Verify:
      Trivy image scan clean locally; Syft produces a valid SPDX document (149 packages).
- [x] 8.2 tools/dependency-licenses: reads every restored src/ project.assets.json plus
      cached nuspecs and package-lock.json, fails on any license token outside the
      allowlist (SPDX OR/AND/WITH expressions allowed when every alternative is). Verify:
      removing MIT from the allowlist fails the check listing the MIT-licensed packages;
      the real graph passes clean.

## 9. Mutation testing (nightly)

- [x] 9.1 dotnet-stryker 5.0.0 in the root dotnet-tools.json;
      tests/Raccord.Domain.Tests/stryker-config.json with thresholds high 80 / low 70 /
      break 60. Verify: first local run completes and reports a baseline score (recorded
      in the tasks/proposal notes).
- [x] 9.2 .github/workflows/mutation.yml: nightly 03:17 UTC run + workflow_dispatch,
      StrykerConfig report artifact upload. Verify: workflow file is valid YAML with the
      local tool restore path matching the other jobs.

## 10. Post-merge runtime probing

- [x] 10.1 .github/workflows/post-merge.yml: on push to main, build the image, boot the
      Compose stack with --wait, zaproxy/action-baseline against localhost:8080,
      teardown with volumes. Verify: compose boots locally with the same POSTGRES_PASSWORD
      env; ZAP action inputs match the action's README.
- [x] 10.2 docs/operations/ops-checklist.md (backup drill, rotation, access review,
      runbook currency, ZAP triage) and the backup-restore-drill backlog issue (#5).
