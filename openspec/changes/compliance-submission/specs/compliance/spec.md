<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-INT-ENABLE-FISCAL-REPORTING" digest="sha256:a4a91b99a567486e5665c9c0a3f0f7ebc4fe06525c7ac9eccbbc10e20424e04e" -->

# Compliance submission

## ADDED Requirements

### Requirement: Submit pending records idempotently

The worker MUST use the immutable record hash as the authority idempotency key and MUST persist the authority result without logging certificate material.

#### Scenario: Retry after a transient failure

- **WHEN** the authority is unavailable
- **THEN** the record remains pending, its attempt metadata is updated, and a later run can retry it

#### Scenario: Authority rejection

- **WHEN** the authority rejects a record
- **THEN** the record becomes rejected and the safe rejection text and authority reference are visible in persistence
