<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Frontend route behavior tests

## ADDED Requirements

### Requirement: Protect high-value route behavior with focused tests

The frontend MUST test user-visible loading, empty, error or mutation behavior for customer, invoice and issuer-settings routes where those states exist.

#### Scenario: Customer creation payload is normalized

- **WHEN** a user submits a customer with only its required legal name
- **THEN** the frontend sends optional fields as null and preserves the legal name

#### Scenario: Issuer save reports success

- **WHEN** a user saves a valid issuer profile
- **THEN** the UI reports a visible saved state
