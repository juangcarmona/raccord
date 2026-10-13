<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-CHANGE-PROJECT-STATUS" digest="sha256:b653f51a2901fb9689b6b7f0357150880ee7658e322411274696709b93d1ce80" -->

# Project planning

## ADDED Requirements

### Requirement: Projects expose a fixed lifecycle status

Projects MUST persist one of Draft, Planned, Active, Completed, or Cancelled and return it in list and detail responses.

#### Scenario: Change project status

- **WHEN** a coordinator updates an existing project's status
- **THEN** the new status is persisted and returned

#### Scenario: Create project

- **WHEN** a project is created without a status
- **THEN** it starts in Draft status
