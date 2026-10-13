<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-CORRECT-ISSUED-INVOICE" digest="sha256:383b5851f22c6f2ab00014f0bffe0e2c4d24bd7efe038bd33a568572ec41804f" -->
<!-- pdac:cite id="BR-RECTIFYING-INVOICE" digest="sha256:e635a992493382932590a0dcb7e97081e6bbe3a7dcb8b4b248da1834f0e168bd" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->

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
