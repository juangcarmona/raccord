<!-- pdac-scope: none reason="Owner-requested repository delivery automation and queue bookkeeping only; no application product semantics change." -->

# Delivery train bootstrap

## ADDED Requirements

### Requirement: Preserve approved delivery scope and serial order

The repository SHALL retain the owner authorization and a revision-bound inventory for the remaining epic delivery queue. The queue SHALL permit one active implementation at a time and SHALL respect issue dependencies across epic boundaries.

#### Scenario: Existing implementation is outside main

- **WHEN** an approved issue has implementation on an existing epic branch
- **THEN** the delivery plan reuses and requalifies that implementation against its declared current base
- **AND** historical issue closure is not treated as current acceptance

### Requirement: Install the published delivery workers consistently

The selected workers SHALL use the exact published SDLC package, compile with the pinned gh-aw version, and run explicit repository implementation checks. Product Change validation/application and existing CI SHALL remain enforced.

#### Scenario: Evidence preparation is unavailable

- **WHEN** the published harness cannot prepare revision-bound product evidence within its supported limits
- **THEN** the queue remains visibly blocked before provider execution
- **AND** the repository SHALL NOT claim that installation or queued labels establish an operating unattended train

### Requirement: Preserve native authorization contracts

The bootstrap SHALL NOT substitute blanket product approval for future native review keys, candidate-head merge decisions, verified legal/protocol evidence, credentials or production authorization.

#### Scenario: Optional epic controller has not been qualified

- **WHEN** the optional native epic workflow is installed without its accepted current plan, protected evidence configuration and native qualification
- **THEN** execution remains disabled
- **AND** the missing prerequisites remain recorded in the train handoff
