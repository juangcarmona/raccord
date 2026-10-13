<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WHS-PLAN-EQUIPMENT" digest="sha256:bf35a40c66972787dc1081df78df5744b0524ed0b4996d682b2831b60a02f3f5" -->
<!-- pdac:cite id="UC-WHS-SET-LINE-TIMES" digest="sha256:d2c7fe17a40ee9b0bfde05c60f5ac8a8bafd5be1b8ca8c5d63ccc8e326d7eee6" -->

# Project equipment planning

## ADDED Requirements

### Requirement: Equipment lines stay within project periods

Planned equipment lines MUST have a positive quantity and dates contained by the project start and end dates.

#### Scenario: Plan equipment

- **WHEN** a coordinator adds an equipment line with an in-range period
- **THEN** the project persists and returns the line

#### Scenario: Reject an out-of-range line

- **WHEN** a coordinator submits a line outside the project period or with a non-positive quantity
- **THEN** the project rejects the request and does not persist the line
