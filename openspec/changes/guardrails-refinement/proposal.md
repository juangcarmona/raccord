## Why

Raccord enforced its quality gates only server-side (CI). A local `--no-verify` or a stale hook
install silently skipped formatting, lint and commit-message checks until PR time, and test
naming had no machine-checked convention, so a failing test's name rarely stated the
expectation that broke. GitHub issue
[#4](https://github.com/juangcarmona/raccord/issues/4).

## What Changes

- **Local hooks**: husky installed as a .NET local tool, activated automatically by the
  HuskyInstall MSBuild target in the root Directory.Build.props (skipped on CI and with
  `HUSKY=0`). `.husky/pre-commit` runs CSharpier on staged `.cs` files and lint-staged
  (ESLint --fix + Prettier) on staged TS/TSX/JSON/MD/YML/CSS; `.husky/commit-msg` enforces
  conventional commits (feat, fix, chore, docs, refactor, test, ci) via commitlint.
- **Formatting**: CSharpier replaces `dotnet format` for C# (style rules stay enforced by
  EnforceCodeStyleInBuild + TreatWarningsAsErrors); Prettier covers the frontend.
- **Test conventions**: assertions migrated to Shouldly; all tests renamed to
  Thing_Should_Outcome and the convention enforced over every test assembly by
  `TestNaming_Should_Constraint` in tests/Raccord.Architecture.Tests.
- **E2E journeys**: a new `Raccord.EndToEndTests` project boots the unmodified Aspire AppHost
  (fresh session-lifetime Postgres per run) and drives the real SPA with Playwright through four
  smoke journeys (boot, customer, billable line, invoice finalize). The per-PR selection is
  pinned by a committed manifest, so a narrowed filter is a red build rather than a quieter
  suite; failed journeys leave a trace and screenshot as CI artifacts.
- **CI mirror**: a lint job re-runs the commit-time gates in `--check` mode (CSharpier,
  Prettier, commitlint over the PR commit range) so a hook bypass is still caught before merge.
- **Arbitrary-bypass note**: direct pushes to main skip the commitlint mirror (it runs on
  `pull_request` only); branch protection is the policy answer.

## Capabilities

### New Capabilities
- `commit-hooks`: local pre-commit and commit-msg hooks, auto-installed on build, with an
  explicit `HUSKY=0` escape.
- `commit-message-convention`: conventional commits with a restricted type-enum, enforced at
  commit time and mirrored in CI.
- `pre-commit-checks`: staged-content checks (CSharpier for C#, lint-staged for the frontend).
- `ci-mirrors`: a CI job re-running every commit-time gate in check mode.
- `test-conventions`: Shouldly assertions and Thing_Should_Outcome test naming, both
  machine-enforced.
- `e2e-testing`: browser journeys over the booted Aspire AppHost, with a manifest-pinned
  per-PR selection and failure evidence as artifacts.

### Modified Capabilities
_(none — no spec'd capability's requirements change; this change is the seed for the
spec layer itself)_

## Impact

- **Repo-wide, developer-experience only**: no runtime or API changes. One-time CSharpier
  reformat (review with `git diff -w`, it is empty) and one-time test renames.
- **Tooling pins**: husky 0.9.1 and csharpier 1.3.0 in the root dotnet-tools.json; Shouldly
  4.3.0, Aspire.Hosting.Testing 13.6.0 and Microsoft.Playwright 1.63.0 in
  Directory.Packages.props; prettier 3.6.2, lint-staged 16.2.3, @commitlint 19.8.1 in the
  frontend devDependencies.
- **CI**: new lint and e2e jobs in .github/workflows/ci.yml; backend job's Format step is
  CSharpier and its test run excludes the E2E project (the e2e job runs it with Docker).
