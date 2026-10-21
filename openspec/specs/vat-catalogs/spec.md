<!-- pdac-scope: cited -->

# VAT Catalogs Specification

## Purpose

Provide administrators with stable VAT class and scheme catalogs, and make configured schemes available to project and contact-default consumers without changing previously used tax rates.

## Requirements

### Requirement: Administrators manage VAT classes and schemes

The system MUST let an authorized administrator create and maintain named VAT classes and schemes that use per-class rates, one fixed rate, or reverse charge. It MUST reject invalid references and unsupported configurations without partial writes.

<!-- pdac:cite id="UC-FIN-CONFIGURE-VAT" digest="sha256:3c05cb10f788f77cb6752fca0c806d089d618a79be13a6cd2a2d4cb911388ab7" -->
<!-- pdac:cite id="FR-FIN-VAT" digest="sha256:bde0ed771d8db4d834acb66d2b278fef674664670ac720dd9c91dbde99e1c7df" -->
<!-- pdac:cite id="TERM-VAT-CLASS" digest="sha256:3ad27a6e017bcec71c1452669ff354b4c0fc8f29725635de0a90c01421d287c2" -->
<!-- pdac:cite id="TERM-VAT-SCHEME" digest="sha256:d7fbf3cf62480bae491180a199e8fbe4455b25712d349af2b203173abf42095f" -->
<!-- pdac:cite id="TERM-VAT-SCHEME-KIND" digest="sha256:b7a466e19572ce3eae02fc09efab7e1adbc34b940bf889d2d32984ee79d508cf" -->
<!-- pdac:cite id="BR-FIN-VAT-FROM-CLASS-AND-SCHEME" digest="sha256:7ec9486b4513c0492fa7b7b53718f358db9717f8b620d1ee9d1f839e791c6f2a" -->

#### Scenario: Administrator creates a per-class scheme

- **WHEN** an authorized administrator saves a scheme that maps configured VAT classes to valid rates
- **THEN** the system stores the scheme with a stable identity and returns its class-rate mapping

#### Scenario: Invalid class reference is rejected atomically

- **WHEN** an administrator saves a scheme containing a class that does not exist
- **THEN** the system rejects the request and persists none of the attempted changes

### Requirement: New Spanish installations receive a source-dated standard-rate default

The system MUST initialize a new Spanish installation with the standard-rate VAT class and default scheme at 21%, using one named project constant whose official source and verification date are stored with the initial configuration. This value MUST remain an editable setup default and MUST NOT classify every operation as subject to the standard rate. The constant MUST NOT overwrite or silently update an installation's saved configuration after initialization.

<!-- pdac:cite id="UC-FIN-CONFIGURE-VAT" digest="sha256:3c05cb10f788f77cb6752fca0c806d089d618a79be13a6cd2a2d4cb911388ab7" -->
<!-- pdac:cite id="TERM-VAT-SCHEME" digest="sha256:d7fbf3cf62480bae491180a199e8fbe4455b25712d349af2b203173abf42095f" -->
<!-- pdac:cite id="BR-FIN-DEFAULT-VAT-CLASS" digest="sha256:7f539a8bdefbc8cea1877191ba869addd68c206993c40a5f791f8bf96a09049b" -->

#### Scenario: Fresh Spanish installation receives the initial default

- **WHEN** a new Spanish installation completes its first setup
- **THEN** it contains the standard-rate class and a default scheme initialized to 21% with the source and verification date recorded
- **AND** it contains no prefilled reduced, zero-rate, or reverse-charge rates

#### Scenario: Saved setup is not replaced by the initialization constant

- **WHEN** the installation starts after an administrator has saved a different standard scheme
- **THEN** initialization preserves the saved scheme and does not reapply the 21% constant

### Requirement: Used rates remain historically stable

The system MUST refuse to change a VAT rate once that rate has been used by a saved commercial record. A project billable line MUST save the stable scheme identity and the effective percentage snapshot on first use, and an invoice draft line MUST carry both forward. Recording scheme use MUST be atomic with saving the commercial record. A later rate change MUST be represented by a new scheme so existing records retain their original scheme and rate.

<!-- pdac:cite id="TERM-VAT-SCHEME" digest="sha256:d7fbf3cf62480bae491180a199e8fbe4455b25712d349af2b203173abf42095f" -->
<!-- pdac:cite id="BR-FIN-USED-RATE-FROZEN" digest="sha256:b33134bd0f2519dfb830cd37b214eab8e6c9e8609b7a9bf63c9db8e2e6cb8a95" -->

#### Scenario: Used VAT rate cannot be edited

- **GIVEN** a saved project billable line records the stable scheme identity and rate snapshot
- **WHEN** an administrator attempts to change that rate
- **THEN** the system rejects the update and preserves the existing scheme, project line and any invoice draft values

#### Scenario: First use and concurrent rate update serialize

- **GIVEN** a project billable line is being saved while an administrator updates the same scheme
- **WHEN** both operations commit
- **THEN** the saved line contains the complete scheme identity and percentage snapshot from one committed scheme version
- **AND** any update after that first use is rejected
- **AND** no committed project line can refer to a rate version that the catalog no longer contains

#### Scenario: Rate change creates a new scheme

- **GIVEN** an existing scheme rate has been used
- **WHEN** an administrator creates a replacement scheme and selects it as the default
- **THEN** new projects can use the new scheme while existing projects retain the original scheme

### Requirement: Consumers address schemes by stable identity

The system MUST expose configured VAT schemes through an authorized catalog contract and API with stable identities so Contacts and Projects can reference configured schemes without copying display names or rates as free text.

<!-- pdac:cite id="FR-FIN-VAT" digest="sha256:bde0ed771d8db4d834acb66d2b278fef674664670ac720dd9c91dbde99e1c7df" -->
<!-- pdac:cite id="TERM-VAT-SCHEME" digest="sha256:d7fbf3cf62480bae491180a199e8fbe4455b25712d349af2b203173abf42095f" -->

#### Scenario: Consumer resolves a scheme by identity

- **WHEN** an authorized contact or project workflow requests the available VAT schemes
- **THEN** the response contains each scheme's stable identity, name, kind, default status, and configured rate information
- **AND** a consumer can save a reference to a returned scheme identity

### Requirement: VAT catalog changes are audited atomically

The system MUST append an audit event for initial catalog setup, class or scheme creation and updates, and default-scheme changes. Each event MUST identify the actor, action, catalog entity and relevant before/after values. The audit event and catalog change MUST commit or roll back together, and audit events MUST remain append-only.

<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="TERM-AUDIT-EVENT" digest="sha256:5f9c5f251cc44d2126d3745e70734839205b6376ebea73190b200f56bfe7ed2d" -->

#### Scenario: Catalog update and audit event commit together

- **WHEN** an administrator changes a VAT scheme or its default status
- **THEN** the catalog mutation and an audit event with the actor and before/after values are committed in the same operation

#### Scenario: Failed catalog update leaves no audit event

- **WHEN** a VAT catalog update fails validation or persistence
- **THEN** the catalog remains unchanged and no audit event for the failed mutation is committed
