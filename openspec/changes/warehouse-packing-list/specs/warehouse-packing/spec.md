<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WHS-MARK-READY-TO-PACK" digest="sha256:d30aa4947b830445ca65df5ad0d5a07abcfdda648a377c522360f8d5cb3a254f" -->

# Warehouse packing

## ADDED Requirements

### Requirement: Create a packing list

The system MUST create one packing list from the project's planned equipment quantities.

#### Scenario: Planned equipment is copied

- **WHEN** a coordinator creates a packing list for a project with planned equipment
- **THEN** each planned equipment line is represented with its quantity and pending state

### Requirement: Pack a line once

The system MUST capture the actor and trusted timestamp when a pending line is packed.

#### Scenario: Pending line is packed

- **WHEN** a coordinator packs a pending line
- **THEN** the line reports packed state, actor and timestamp

#### Scenario: Packed line is not packed twice

- **WHEN** a coordinator packs an already packed line
- **THEN** the operation fails with `projects.packing-already-complete`
