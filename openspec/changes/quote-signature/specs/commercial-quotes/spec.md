<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-GENERATE-QUOTE" digest="sha256:56ae41de1ef841ad1d5e386acfe32ab75cde55b2e0f416d519cee98138be69a1" -->
<!-- pdac:cite id="FR-FIN-QUOTES" digest="sha256:19bbedb0836b7f972239790fff9ee80f056cd86c79e44409932aa79021a2b0fa" -->

# Commercial quote signatures

## ADDED Requirements

### Requirement: Record quote signature metadata

The system MUST record signer display name and trusted timestamp only for an accepted quote.

#### Scenario: Accepted quote is signed

- **WHEN** the coordinator signs an accepted quote with a non-empty signer name
- **THEN** the quote reports `SignatureRecorded`
- **AND** signer name and timestamp are exposed

#### Scenario: Unaccepted quote cannot be signed

- **WHEN** the coordinator signs a quote before acceptance
- **THEN** the operation fails with `quotes.signature-invalid`
