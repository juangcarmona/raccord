<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Apply the approved numbering definition through its provider

Recovery MUST validate and explicitly apply CHG-CONTACT-NUMBER-SERIES-397 through ProductShape on the review branch.

<!-- pdac:cite id="FR-WSP-030" digest="sha256:3106f257093a9439e1653c31c1876a6945d04ea9e4880be382388d0a9c41253d" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e" -->

#### Scenario: Approved historical proposal is reconciled on the current baseline

- **GIVEN** the original owner-approved change and the accepted main product baseline
- **WHEN** the maintainer reconciles the proposed artifacts and validates the overlay
- **THEN** all four direct impacts of the modified rule have current content/cause digests and specific dispositions
- **AND** the overlay reports zero errors and warnings before explicit provider application
- **AND** the applied diff adds exactly four product artifacts and modifies only the existing assignment rule
- **AND** the applied decision record is retained while redundant proposed copies are removed
- **AND** branch application is not reported as main acceptance or runtime delivery

#### Scenario: Native evidence limitation remains visible

- **GIVEN** native preparation reports the Workspace impact report unavailable because of its serialization bound
- **WHEN** the maintainer validates and applies the same change using the complete installed provider graph
- **THEN** the native result remains blocked and is not rebound to the operator candidate
- **AND** the operator evidence records its actual validation and application separately
- **AND** the runtime order and future hold remain in force

<!-- pdac:cite id="BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION" digest="sha256:d6491901cbe82003e5ce323b7a979d0a2a6c33be2383d97e5db0f65920206371" -->
<!-- pdac:cite id="SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION" digest="sha256:1c6c3ce42e36bc172f3b6e66bc595e0223a0673b03bfe62dba9978176a0f21f7" -->
