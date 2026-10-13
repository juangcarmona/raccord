<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-CONFIGURE-FINANCIAL-SETTINGS" digest="sha256:c8d6c60d06a57fc49ca312afeb07badd80ffa259b9a23e76a97f09a8a52537df" -->

# Financial settings

## ADDED Requirements

### Requirement: Store one validated company-wide settings row

The system MUST store quote and contract validity in days and an explicit expired-signing policy for the installation.

#### Scenario: Reject invalid validity

- **WHEN** either validity period is zero or negative
- **THEN** the update is rejected with a domain rule code and no settings are saved
