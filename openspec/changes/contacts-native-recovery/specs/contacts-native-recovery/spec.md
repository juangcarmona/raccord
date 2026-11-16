<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Supply immutable approved recovery source

The native recovery for FR-CON-002 SHALL receive a validated issue-specific seed whose declared base/head and current-source preimages pass the installed provider contract. It SHALL preserve the reconciled product baseline.

#### Scenario: Historical code is absent from the materialized snapshot

- **WHEN** issue 299 starts from the current main baseline
- **THEN** its candidate contains the approved directory/person recovery input
- **AND** the worker requalifies that input against current checks

### Requirement: Preserve independent checks without exhausting private scratch

The adopter SHALL retain its existing independent checks and store .NET package/HTTP caches in the disposable ignored workspace obj tree.

#### Scenario: Fresh solution restoration exceeds two GiB of cache

- **WHEN** the pinned implementation image restores the solution with its existing private tmpfs limit
- **THEN** packages and HTTP cache use the declared workspace paths
- **AND** the check retains its real exit status
- **AND** no candidate, merge or acceptance is inferred from a configuration check alone

### Requirement: Run real checks with owned disposable services

The isolated workload SHALL use a provider-owned PostgreSQL service without Docker access. Web tests SHALL keep distinct databases; the real Aspire AppHost and Playwright suite SHALL use a fresh database and installation-local file storage in Testing mode. Failed provisioning SHALL fail checks rather than skip them.

#### Scenario: Docker is unavailable inside the workload

- **WHEN** the complete backend and pinned browser selection run with the disposable server connection
- **THEN** every test retains its existing behavior assertions
- **AND** the fixtures create and clean up only their owned databases and document directories
- **AND** development AppHost behavior retains its PostgreSQL and Azurite containers

### Requirement: Preserve terminal managed command evidence

The worker SHALL invoke managed commands without redirecting to the wrapper-owned log and SHALL inspect a terminal receipt before starting another command. A failed prelaunch receipt SHALL remain available while a corrected invocation uses a new identifier.

#### Scenario: Wrapper owns the command log

- **WHEN** installation starts through the managed command wrapper
- **THEN** the shell does not pre-create the wrapper log
- **AND** the receipt and actual command exit status remain attributable to that invocation

### Requirement: Keep browser caches out of source inventory

The native check workspace SHALL ignore its generated root .cache/ms-playwright browser cache without weakening source bounds or hiding tracked source edits.

#### Scenario: Browser qualification installs Chromium in the disposable workspace

- **WHEN** the complete configured browser command downloads its runtime
- **THEN** downloaded binaries remain disposable output outside the implementation source inventory
- **AND** tracked source edits remain visible to verification
- **AND** acceptance still requires terminal managed receipts and authentic native publication

<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
