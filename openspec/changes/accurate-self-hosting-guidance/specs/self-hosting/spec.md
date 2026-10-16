<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Self-hosting guidance matches current authentication

The canonical self-hosting guide SHALL describe the supported Compose installation and first-administrator setup. It SHALL state that the first administrator is created through a one-time unauthenticated setup endpoint, that sign-in is required after setup, and that no public registration is available. It SHALL direct operators to perform first-user setup on a trusted network before broader access and retain TLS, database isolation, backup and upgrade guidance.

#### Scenario: An operator installs a fresh instance

- **WHEN** an operator follows the canonical self-hosting guide on a new Compose installation
- **THEN** the operator can start the services, check health, create the first administrator, sign in and reach issuing-company setup using commands that match the repository

#### Scenario: An operator evaluates internet exposure

- **WHEN** an operator reads the guide before exposing the instance
- **THEN** the guide distinguishes one-time initial setup from ongoing sign-in, warns against exposing the uninitialized setup endpoint, and requires TLS while keeping PostgreSQL unpublished

<!-- pdac:cite id="ACT-INSTALLATION-ADMINISTRATOR" digest="sha256:2a214515187fede3e5d9b093a24cb298d21db07c0300f2772dfcccf1d8ff0815" -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
