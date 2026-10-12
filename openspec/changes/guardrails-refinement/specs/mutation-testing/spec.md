<!-- pdac-scope: none reason="Repository engineering tooling and CI process; implements no product behaviour defined in docs/product/model." -->

# mutation-testing Specification

## Purpose

Keeps the test suite honest about the code it claims to cover: Stryker mutates the Domain layer on a nightly schedule and fails on a mutation score below the break threshold. Nightly rather than per-PR until the suite is dense enough for a stable score — a gate earns its place by catching something, and a flaky score trains people to ignore it.

## ADDED Requirements

### Requirement: nightly Stryker run scores the Domain mutation coverage

A scheduled workflow SHALL run Stryker over the Raccord.Domain test project with the committed `tests/Raccord.Domain.Tests/stryker-config.json` and the `dotnet-stryker` local tool, and SHALL fail when the mutation score is below the break threshold.

#### Scenario: the suite lets too many mutants survive

- **WHEN** the nightly run reports a mutation score below the break threshold
- **THEN** the mutation job fails and the report artifact shows the surviving mutants

#### Scenario: the run completes above the threshold

- **WHEN** the nightly run reports a score at or above break
- **THEN** the job passes and the HTML report is uploaded as a workflow artifact with a bounded retention window

### Requirement: the tool and config are pinned in the repository

The mutation tooling SHALL be pinned as a .NET local tool in the root `dotnet-tools.json`, and its thresholds SHALL live in a committed config file under the Domain test project.

#### Scenario: a fresh checkout can run the mutation suite

- **WHEN** `dotnet tool restore` runs on a fresh clone
- **THEN** `dotnet stryker --config-file tests/Raccord.Domain.Tests/stryker-config.json` reproduces the same configured thresholds and reporters CI uses
