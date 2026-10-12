<!-- pdac-scope: none reason="Repository engineering tooling and CI process; implements no product behaviour defined in docs/product/model." -->

## Purpose

A commit's staged content already satisfies the formatters and linters, so no unformatted or
lint-failing code reaches history even when the author never runs them manually.

## ADDED Requirements

### Requirement: Staged C# files are CSharpier-formatted

The pre-commit hook SHALL run CSharpier on staged `.cs` files and re-stage the formatted result.

#### Scenario: Misformatted staged file

- **WHEN** a `.cs` file with wrong formatting is staged and committed
- **THEN** the committed version is CSharpier-formatted

### Requirement: Staged frontend files pass ESLint and Prettier

The pre-commit hook SHALL run lint-staged against staged frontend files: `eslint --fix --max-warnings=0` then `prettier --write` for `.ts`/`.tsx`, and `prettier --write` for `.json`/`.md`/`.yml`/`.yaml`/`.css`.

#### Scenario: Lint error survives autofix

- **WHEN** a staged `.ts` file contains an ESLint error that `--fix` cannot repair
- **THEN** the commit is aborted

#### Scenario: Unformatted JSON

- **WHEN** a staged `.json` file is not Prettier-formatted
- **THEN** the committed version is Prettier-formatted

### Requirement: Generated API artifacts are never rewritten

The pre-commit pipeline SHALL NOT run Prettier over files excluded by .prettierignore — at minimum the generated OpenAPI artifacts (openapi/ and src/shared/api/schema.d.ts) and the package lockfile.

#### Scenario: Generated schema staged

- **WHEN** the generated `schema.d.ts` is staged alongside frontend edits
- **THEN** it is not reformatted by the hook
