<!-- pdac-scope: cited -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->

# Compliance record chain

## ADDED Requirements

### Requirement: Issued compliance records are chained

Every issued invoice compliance record MUST store a SHA-256 record hash and the predecessor record hash, and the record MUST be created in the same unit of work as the invoice.

#### Scenario: Issue an invoice

- **WHEN** an invoice is issued
- **THEN** exactly one pending compliance record is created with the invoice content hash and a chained record hash

#### Scenario: Verify a chain link

- **WHEN** a record's predecessor, invoice hash, or identity is changed
- **THEN** recomputing its record hash detects the alteration
