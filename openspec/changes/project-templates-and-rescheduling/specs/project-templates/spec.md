<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-CREATE-PROJECT-TEMPLATE" digest="sha256:a22829e84511d226d7863a063a20ddaa3332df9282fef5a203816d4bee27da38" -->
<!-- pdac:cite id="UC-PRJ-CREATE-FROM-TEMPLATE" digest="sha256:16823e8d642041d00c4820d07e05bcd675ecb4c23a4402fc20547ba6dca3f1b3" -->
<!-- pdac:cite id="UC-PRJ-SHIFT-PROJECT-TIMES" digest="sha256:3fa803cf5608b13a2a019e4ef443ea1d76ec1fac969d94c1ad96f9b9a930cba9" -->

# Project templates and rescheduling

## ADDED Requirements

### Requirement: Projects can be created from reusable templates

The API MUST persist a named template and MUST create a project using the template's customer while applying the requested project name and period.

#### Scenario: Create from an existing template

- **WHEN** a coordinator submits a valid template identifier, name, and period
- **THEN** the system creates a project for the template's customer and returns the project representation

#### Scenario: Unknown template

- **WHEN** a coordinator submits an unknown template identifier
- **THEN** the system returns not found and does not create a project

### Requirement: Rescheduling moves project-owned planning periods

The API MUST apply one date offset to the project period and every planned equipment line in the project.

#### Scenario: Shift a project

- **WHEN** a coordinator shifts a project by an offset in days
- **THEN** the project and all planned equipment lines move by exactly that offset
- **AND** availability reads use the new persisted dates

#### Scenario: Missing project

- **WHEN** a coordinator shifts an unknown project
- **THEN** the system returns not found
