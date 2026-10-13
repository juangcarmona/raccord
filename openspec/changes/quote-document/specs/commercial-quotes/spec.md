<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-GENERATE-QUOTE" digest="sha256:56ae41de1ef841ad1d5e386acfe32ab75cde55b2e0f416d519cee98138be69a1" -->
<!-- pdac:cite id="FR-FIN-QUOTES" digest="sha256:19bbedb0836b7f972239790fff9ee80f056cd86c79e44409932aa79021a2b0fa" -->

# Commercial quote document

## ADDED Requirements

### Requirement: Download an accepted quote

The system MUST return a deterministic document containing the accepted quote number, snapshots and copied lines.

#### Scenario: Accepted quote is downloaded

- **WHEN** an accepted quote document is requested
- **THEN** the response is a UTF-8 text file with a stable quote filename

#### Scenario: Unaccepted quote is not downloadable

- **WHEN** a draft or review-ready quote document is requested
- **THEN** the response is not available
