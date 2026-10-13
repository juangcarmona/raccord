<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-ADD-CONTACT-PERSON" digest="sha256:d692c11d96c5a6a623b821ad083c2da17477b41c8ea8ec165b1c52c50d134c0a" -->

# Contact persons

## ADDED Requirements

### Requirement: A contact has contact persons

The system MUST allow contact persons to be listed and created for a contact.

#### Scenario: Add a contact person

- **WHEN** a valid person is added to an existing contact
- **THEN** the person is returned with its designation and contact details

### Requirement: One default contact person

The system MUST keep at most one default contact person for each contact.

#### Scenario: Replace the default

- **WHEN** a new person is created as default
- **THEN** the previous default is cleared
