<!-- pdac-scope: cited -->

# Self-hosted release channel

## ADDED Requirements

### Requirement: Operators can select an official version

The maintained self-hosting instructions SHALL identify Docker Compose as the supported self-host installation path and explain how to pin an exact official application image version. The project SHALL distinguish official release images from source-built or modified images.

#### Scenario: Install a selected release

- **WHEN** an operator follows the clean installation guide
- **THEN** the Compose configuration runs the selected official image and applies its migrations before the web service starts

### Requirement: Upgrades preserve business records

The upgrade procedure SHALL verify a database and document backup before migration. It SHALL apply migrations before replacing the running web application and SHALL describe restoration of both stores as recovery when migration or post-upgrade health verification fails.

#### Scenario: Migration fails

- **WHEN** the selected release migration exits unsuccessfully
- **THEN** the new web image is not started and the operator is directed to restore the pre-upgrade database and document backup before retrying

### Requirement: Support scope is explicit

The release policy SHALL state that only the latest stable release receives support and security fixes, and SHALL identify Aspire as a development path rather than an end-user release installer.

#### Scenario: Operator uses an older release

- **WHEN** an installation is behind the latest stable release
- **THEN** the operator is directed to upgrade before receiving support

<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
