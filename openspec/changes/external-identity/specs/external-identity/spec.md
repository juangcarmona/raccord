# External identity

## Requirements

### Requirement: OIDC is disabled by default

The system MUST keep external identity disabled unless `Raccord:Authentication:Oidc:Enabled` is true.

#### Scenario: Existing local authentication

- **WHEN** OIDC is disabled
- **THEN** local password sign-in remains available and no external challenge is exposed

### Requirement: Existing accounts only

The system MUST link an external login to an existing local account and MUST NOT create a local account implicitly.

#### Scenario: Unlinked external identity

- **WHEN** an external identity completes authentication without a linked local login
- **THEN** the request is unauthorized and no user is created

### Requirement: Role SSO enforcement

The system MUST reject password sign-in for a user whose role requires external sign-in.

#### Scenario: Password sign-in for an SSO role

- **WHEN** a user in an SSO-required role signs in with a password
- **THEN** the password session is immediately rejected
- **AND** external sign-in remains available for that user

<!-- pdac-scope: cited -->
