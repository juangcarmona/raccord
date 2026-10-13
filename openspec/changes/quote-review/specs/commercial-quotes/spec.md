<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-GENERATE-QUOTE" digest="sha256:56ae41de1ef841ad1d5e386acfe32ab75cde55b2e0f416d519cee98138be69a1" -->
<!-- pdac:cite id="FR-FIN-QUOTES" digest="sha256:19bbedb0836b7f972239790fff9ee80f056cd86c79e44409932aa79021a2b0fa" -->

# Commercial quote review

## ADDED Requirements

### Requirement: Mark a quote ready for signature

The system MUST expose the quote review state and allow a draft quote to become ready for signature without changing copied pricing.

#### Scenario: Draft becomes ready

- **WHEN** the coordinator updates a draft quote with `ReadyForSignature`
- **THEN** the quote reports `ReadyForSignature`

#### Scenario: Transition cannot repeat

- **WHEN** the coordinator updates an already ready quote
- **THEN** the operation fails with `quotes.review-transition-invalid`
