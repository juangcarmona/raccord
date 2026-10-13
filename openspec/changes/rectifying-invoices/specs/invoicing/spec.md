# Invoicing rectification

## ADDED Requirements

### Requirement: Start a rectification from an issued invoice

The system MUST create a new rectifying draft only from an existing issued invoice, preserve the original invoice ID, record a non-empty reason and use series `R`.

#### Scenario: Differences

- **WHEN** a billing administrator starts a differences rectification
- **THEN** the draft contains signed correction lines, references the original and remains independent from it

#### Scenario: Substitution

- **WHEN** a billing administrator starts a substitution rectification
- **THEN** the draft contains a copy of the original lines, references the original and remains independent from it

### Requirement: Preserve issued invoice immutability

The system MUST never modify or delete the original invoice when a rectifying draft is created or issued.

#### Scenario: Original remains unchanged

- **WHEN** a rectifying invoice is issued
- **THEN** the original invoice content and content hash remain unchanged
