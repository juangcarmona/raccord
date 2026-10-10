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
      Raccord.*.Tests assemblies. Verify: a planted badly-named test fails the rule naming the
      offender.

## 5. Spec layer

- [ ] 5.1 Author openspec/config.yaml (this change) and validate it in CI. Verify:
      `npx -y @fission-ai/openspec@1.12.0 validate --all --no-interactive` passes locally and
      in the openspec CI job.
