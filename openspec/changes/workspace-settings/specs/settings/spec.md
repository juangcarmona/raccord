<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SET-LANGUAGE" digest="sha256:4e1830ebc5acc09bdedf5900f8543f6d29ee5a13439f608dfb055a43bf972aed" -->

# Workspace settings

## ADDED Requirements

### Requirement: Settings require organization permissions

Company and workspace settings MUST require the corresponding organization permission.

#### Scenario: Administrator saves preferences

- **WHEN** a user with organization edit permission submits valid settings
- **THEN** the settings are persisted and returned

### Requirement: Issued numbering is stable

Changing settings MUST NOT alter the existing invoice series definitions or numbers already assigned to issued invoices.

#### Scenario: Preferences change after invoicing

- **WHEN** language or time zone is updated
- **THEN** existing invoice-series records remain unchanged
