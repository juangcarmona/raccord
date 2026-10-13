<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-ASSIGN-ROLE" digest="sha256:7225266488941b418540e36431ed75cb596efdb9cddef5109aa52f65a9db9b37" -->

# Role permissions

## ADDED Requirements

### Requirement: Current endpoints require declared permissions

Every protected current-module endpoint MUST require a declared `module:action` permission and return 403 when the signed-in user's role does not grant it.

#### Scenario: User lacks the endpoint permission

- **WHEN** an authenticated user calls an endpoint without its required permission
- **THEN** the endpoint returns HTTP 403

### Requirement: Installation roles have product-defined profiles

Every installation MUST start with Admin, Office, Warehouse, and Crew roles whose permissions match the product model profile.

#### Scenario: First administrator setup completes

- **WHEN** the first administrator is created
- **THEN** all four predefined roles and their permission claims exist

### Requirement: Role assignment is singular and immediate

An administrator MUST be able to assign exactly one role to a user, and the next request MUST use the new role without requiring a new sign-in.

#### Scenario: Administrator changes a user's role

- **WHEN** the administrator assigns a different role
- **THEN** the user has only the new role and its permissions apply on the next request

### Requirement: Administrators can maintain roles

An administrator with role-edit permission MUST be able to inspect, create, and edit roles through the role API and web editor.

#### Scenario: Administrator creates a custom role

- **WHEN** the administrator submits a unique name and selected permissions
- **THEN** the role appears in the role list with exactly those permissions
