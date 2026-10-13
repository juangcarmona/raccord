# External identity

## ADDED Requirements

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
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="UC-WSP-ENFORCE-SSO" digest="sha256:a402b4279d39c467098866fe83778275580e44742ae06be035a9441f44a4e1bc" -->
<!-- pdac:cite id="UC-WSP-RESOLVE-SSO-NONCOMPLIANCE" digest="sha256:5f38b0e2f9b76ac7379b9911e20594c216f5b1009e0a32722e9edb28b90e6397" -->
