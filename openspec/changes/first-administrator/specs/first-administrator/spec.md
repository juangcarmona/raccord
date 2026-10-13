<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: A fresh installation offers first-administrator setup

The application MUST report setup as required while no local user exists and MUST create the first user with the Admin role while seeding the four predefined roles.

#### Scenario: First setup

- **WHEN** the first person submits a valid email and password to the setup endpoint
- **THEN** one user exists, that user has the Admin role, and Admin, Office, Warehouse and Crew roles exist

### Requirement: Completed setup cannot be repeated

The application MUST refuse setup after any user exists and MUST not create another user.

#### Scenario: Second setup attempt

- **WHEN** a person submits setup details after the first administrator exists
- **THEN** the response is 409 and the user count remains unchanged

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:e5ee7da50f5c081b1c257461484a9e6eecfbce0f108a5abd2d388c14c8e4f16a" -->

<!-- pdac:cite id="TERM-PREDEFINED-ROLE" digest="sha256:40c8223391cb84a39ccfa1047f4cec3e38063e401caf61b52a31a2d14d69e7c5" -->
