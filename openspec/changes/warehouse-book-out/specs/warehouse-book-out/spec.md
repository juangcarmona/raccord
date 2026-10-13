<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WHS-MARK-READY-TO-PACK" digest="sha256:d30aa4947b830445ca65df5ad0d5a07abcfdda648a377c522360f8d5cb3a254f" -->

# Warehouse booking out

## ADDED Requirements

### Requirement: Book packed equipment out

The system MUST allow a packed line to be booked out once and record the actor and trusted timestamp.

#### Scenario: Packed line leaves the warehouse

- **WHEN** a coordinator books out a packed line
- **THEN** the line reports booked-out state, actor and timestamp

#### Scenario: Pending line cannot leave

- **WHEN** a coordinator books out a pending line
- **THEN** the operation fails with `projects.book-out-not-packed`

#### Scenario: Booked-out line cannot leave twice

- **WHEN** a coordinator books out an already booked-out line
- **THEN** the operation fails with `projects.book-out-already-complete`
