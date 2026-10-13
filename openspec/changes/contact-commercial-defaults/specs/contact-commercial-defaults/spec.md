<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS" digest="sha256:e798a64b5810d27f4322c6f6e7240aa007a8797d0be79d22d25b791a17b8e7a3" -->

# Contact commercial defaults

## ADDED Requirements

### Requirement: New projects copy contact defaults

The system MUST copy a contact's commercial defaults into a project when the project is created.

#### Scenario: Contact default changes later

- **WHEN** a contact default changes after a project is created
- **THEN** the existing project retains its copied value
