<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-EQP-IMPORT-EQUIPMENT" digest="sha256:04045c947021522a7381e7289dc2ef128763188b8e4179969c4bcb4a7335324c" -->

# Equipment import

## ADDED Requirements

### Requirement: Equipment CSV imports report row-level outcomes

The import endpoint MUST validate each row and MUST NOT persist invalid rows.

#### Scenario: Import valid and invalid rows

- **WHEN** a CSV contains valid rows and rows with missing fields, invalid types, or duplicate codes
- **THEN** valid rows are imported and rejected rows return their row numbers and reasons

### Requirement: Equipment imports require a stable header

The import MUST reject files that do not use the documented `code,name,type,stockQuantity` header.

#### Scenario: Reject an unknown format

- **WHEN** the first row is not the documented header
- **THEN** no equipment row is imported and the response explains the expected header
