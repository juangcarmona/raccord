<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Invoice issue date

## ADDED Requirements

### Requirement: The server owns the default legal issue date

The invoice draft UI MUST NOT derive an untouched issue date from the browser clock.

#### Scenario: Browser and organization are on different dates

- **WHEN** a user finalizes a valid draft without selecting an issue date
- **THEN** the frontend sends a null issue date and the server applies today in the organization timezone

#### Scenario: User selects an issue date

- **WHEN** a user selects a valid issue date before finalizing
- **THEN** the frontend sends that date unchanged

#### Scenario: Server validation rejects an invalid date

- **WHEN** the selected date violates invoice numbering or date-order rules
- **THEN** the server rejects finalization and the existing error is shown
