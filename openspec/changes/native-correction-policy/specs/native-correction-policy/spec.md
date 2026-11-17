<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Qualify corrections under the trusted service policy

Native correction checks SHALL use the default-branch declaration of an owned disposable PostgreSQL service, with the existing .NET commands mapped to its connection and all existing checks preserved.

#### Scenario: A recovered runtime cannot access workload Docker

- **WHEN** the authenticated correction checks run on the current PR head
- **THEN** HTTP and real AppHost checks use the disposable service without a Docker socket
- **AND** per-test database cleanup and the unchanged browser selection remain required
- **AND** wrapper-managed logs are not pre-created by shell redirection
- **AND** readiness still requires authentic native evidence and green required checks for the actual head

<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
