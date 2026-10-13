<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-GENERATE-QUOTE" digest="sha256:56ae41de1ef841ad1d5e386acfe32ab75cde55b2e0f416d519cee98138be69a1" -->
<!-- pdac:cite id="FR-FIN-QUOTES" digest="sha256:19bbedb0836b7f972239790fff9ee80f056cd86c79e44409932aa79021a2b0fa" -->

# Commercial quotes

## ADDED Requirements

### Requirement: Generate an immutable quote snapshot

The system MUST copy the current project's priced billable lines, customer identity, project identity, payment terms and a validity date into a newly numbered quote.

#### Scenario: Plan is copied at generation time

- **WHEN** a quote is generated for a project with priced lines
- **THEN** the response contains those lines and a `Qyyyy-00000` number
- **AND** the validity date is issue date plus the configured quote validity days

#### Scenario: Empty plans are rejected

- **WHEN** a quote is generated for a project with no priced lines
- **THEN** the operation fails with `quotes.plan-empty`

### Requirement: Quote snapshots remain stable

The system MUST return the stored snapshot without reading current project pricing when a quote is retrieved.

#### Scenario: Plan changes after generation

- **WHEN** a project line changes after quote generation
- **THEN** the existing quote retains its original copied values
