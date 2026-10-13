<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WHS-MARK-READY-TO-PACK" digest="sha256:d30aa4947b830445ca65df5ad0d5a07abcfdda648a377c522360f8d5cb3a254f" -->

# Warehouse exceptions

## ADDED Requirements

### Requirement: Record a received equipment exception

The system MUST allow one loss or damage exception after receipt with kind, description, actor and trusted timestamp.

#### Scenario: Received line gets an exception

- **WHEN** a coordinator reports loss or damage on a received line
- **THEN** the exception is visible with its details and metadata

#### Scenario: Unreceived line cannot get an exception

- **WHEN** a coordinator reports an exception before receipt
- **THEN** the operation fails with `projects.exception-not-received`

#### Scenario: An exception is recorded once

- **WHEN** a coordinator reports a second exception on the line
- **THEN** the operation fails with `projects.exception-already-recorded`
