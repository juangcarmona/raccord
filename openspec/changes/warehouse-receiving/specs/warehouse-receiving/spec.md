<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WHS-MARK-READY-TO-PACK" digest="sha256:d30aa4947b830445ca65df5ad0d5a07abcfdda648a377c522360f8d5cb3a254f" -->

# Warehouse receiving

## ADDED Requirements

### Requirement: Receive booked-out equipment

The system MUST allow a booked-out line to be received once and record actor and trusted timestamp.

#### Scenario: Booked-out line is received

- **WHEN** a coordinator receives a booked-out line
- **THEN** the line reports received state, actor and timestamp

#### Scenario: Unbooked line cannot be received

- **WHEN** a coordinator receives a line that has not left the warehouse
- **THEN** the operation fails with `projects.receive-not-booked-out`

#### Scenario: Received line cannot be received twice

- **WHEN** a coordinator receives an already received line
- **THEN** the operation fails with `projects.receive-already-complete`
