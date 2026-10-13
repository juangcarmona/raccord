<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-GENERATE-QUOTE" digest="sha256:56ae41de1ef841ad1d5e386acfe32ab75cde55b2e0f416d519cee98138be69a1" -->
<!-- pdac:cite id="FR-FIN-QUOTES" digest="sha256:19bbedb0836b7f972239790fff9ee80f056cd86c79e44409932aa79021a2b0fa" -->

# Commercial quote acceptance

## ADDED Requirements

### Requirement: Record quote acceptance

The system MUST record acceptance only for a quote ready for signature and MUST preserve the original quote number and pricing.

#### Scenario: Ready quote is accepted

- **WHEN** the coordinator accepts a ready quote
- **THEN** the quote reports `Accepted`
- **AND** `AcceptedAt` is populated from the trusted application clock

#### Scenario: Draft quote cannot be accepted

- **WHEN** the coordinator accepts a draft quote
- **THEN** the operation fails with `quotes.acceptance-invalid`
