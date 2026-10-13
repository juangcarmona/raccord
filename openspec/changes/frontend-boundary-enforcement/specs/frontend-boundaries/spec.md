<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Frontend boundaries

## ADDED Requirements

### Requirement: Enforce frontend dependency direction

The frontend MUST enforce `app → features → shared` imports in CI.

#### Scenario: A forbidden import is introduced

- **WHEN** shared imports app/features, a feature imports app, or a feature imports a peer feature
- **THEN** the architecture check fails with the offending source path

#### Scenario: An allowed composition is used

- **WHEN** app composes features or shared code, a feature uses shared code, or shared imports shared code
- **THEN** the architecture check passes
