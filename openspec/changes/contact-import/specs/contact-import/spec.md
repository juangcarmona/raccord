<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-IMPORT-CONTACTS" digest="sha256:4af29a38f956f50cc0595520b37addfd90cc559dd74db74be9d76c380fe69105" -->

# Contact import

## ADDED Requirements

### Requirement: Invalid imports are atomic

The system MUST reject invalid rows and MUST NOT persist any valid rows from the same upload.

#### Scenario: Upload contains an invalid row

- **WHEN** a CSV upload contains a malformed row
- **THEN** the response identifies the rejected row
- **AND** no contact from that upload is persisted
