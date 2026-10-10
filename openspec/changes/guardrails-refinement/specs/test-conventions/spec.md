## Purpose

Test code follows one assertion library and one naming shape, so any test failure is
immediately readable: the name states the expectation that broke, and the assertion message
comes from Shouldly.

## ADDED Requirements

### Requirement: Tests use Shouldly assertions
Every test project SHALL reference Shouldly (4.3.0, centralized in Directory.Packages.props, wired once from tests/Directory.Build.props with a global using) and test code SHALL use its fluent assertions rather than xUnit's Assert API.

#### Scenario: New test file
- **WHEN** a new test is written
- **THEN** its assertions use Should Be/Throw/Contain fluent forms available without extra using directives

### Requirement: Test names read as Thing_Should_Outcome
Every [Fact] or [Theory] method name SHALL match `^Thing_Should_Outcome$` in snake-case segments, or its declaring type name SHALL match the pattern. A repository-wide constraint test SHALL enforce it by reflecting over every Raccord.*.Tests assembly.

#### Scenario: Conforming name
- **WHEN** a test method is named `A_draft_Should_require_at_least_one_line`
- **THEN** the naming constraint passes

#### Scenario: Offending name
- **WHEN** a test method is named `DraftWorks` with a declaring type that also does not match
- **THEN** the naming constraint fails, listing `DraftTests.DraftWorks` as an offender

#### Scenario: New test project is covered
- **WHEN** a new Raccord.*.Tests project is added to the solution
- **THEN** its DLL lands in Architecture.Tests' output directory (via ProjectReference) and its tests are subject to the same rule
