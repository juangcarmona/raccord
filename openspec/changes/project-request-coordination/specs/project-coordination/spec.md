<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-RECEIVE-PROJECT-REQUEST" digest="sha256:dd19f461f00bac47b39603582fa87cdd8f3ad836ca659c0fce729ca27b1e7050" -->
<!-- pdac:cite id="UC-PRJ-CONVERT-PROJECT-REQUEST" digest="sha256:7efe604979d08b83572e4bf7e9441cef0e69f20f7e51645a99419d611c473a50" -->
<!-- pdac:cite id="UC-PRJ-DECLINE-PROJECT-REQUEST" digest="sha256:2dd0a6ef48a12bda0d07f5a6f6157f7e9216441b41e4babed04745024483e957" -->

# Project coordination

## ADDED Requirements

### Requirement: Project requests have an auditable lifecycle

The system MUST retain incoming project requests and their source contact details while allowing them to be converted or declined exactly once.

#### Scenario: Convert a request

- **WHEN** a received request has a linked customer
- **THEN** the system creates a project with the requested period and classifications and marks the source request converted

#### Scenario: Decline a request

- **WHEN** a coordinator declines a received request
- **THEN** the system requires and retains the decline reason

### Requirement: Projects can use reusable classifications

The system MUST allow project types and teams to be configured and assigned to project requests and projects.

#### Scenario: Assign reusable classifications

- **WHEN** a coordinator receives or creates a project with a configured type and team
- **THEN** those identifiers remain attached to the request and resulting project
