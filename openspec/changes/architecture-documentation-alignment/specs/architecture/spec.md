<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Architecture documentation

## ADDED Requirements

### Requirement: Architecture documentation reflects implemented boundaries

The architecture documentation MUST describe implemented authentication, frontend route protection, module composition and host responsibilities accurately.

#### Scenario: Authentication is implemented

- **WHEN** a contributor reads the security and runtime views
- **THEN** the documentation describes the Identity cookie session, protected API and public sign-in/health boundaries

#### Scenario: Role authorization remains future work

- **WHEN** a contributor reads the current risks and scope
- **THEN** role-based authorization is identified as remaining work rather than being implied to exist
