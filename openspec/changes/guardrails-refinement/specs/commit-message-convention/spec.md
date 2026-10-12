<!-- pdac-scope: none reason="Repository engineering tooling and CI process; implements no product behaviour defined in docs/product/model." -->

## Purpose

Every commit message follows the conventional-commits shape with a restricted type enum, so
history reads as a changelog and tooling can rely on the type prefix.

## ADDED Requirements

### Requirement: Commit messages follow conventional commits with a restricted type enum

Each commit message SHALL match `<type>(<optional scope>): <subject>` where type is one of feat, fix, chore, docs, refactor, test, ci. The rule SHALL be enforced at commit time by commitlint.

#### Scenario: Conforming message

- **WHEN** a commit message is `feat(invoicing): add draft finalization`
- **THEN** the commit is accepted

#### Scenario: Non-conforming message

- **WHEN** a commit message is `update stuff`
- **THEN** the commit is rejected by the commit-msg hook

#### Scenario: Disallowed type

- **WHEN** a commit message is `style: reformat files`
- **THEN** the commit is rejected because `style` is not in the type enum

### Requirement: The convention applies to every commit, not only frontend changes

The commitlint configuration SHALL live at the repository root (.config/commitlint.config.cjs) and validate every commit made in the repository.

#### Scenario: Backend-only commit

- **WHEN** a commit touches only `.cs` files but has a non-conforming message
- **THEN** the commit is rejected
