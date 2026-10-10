# license-policy Specification

## Purpose

Keeps the dependency tree distribution-safe for Raccord's AGPL + brand-independence
position (ADR-0002): a license outside the allowlist must block the build before it
ships in a deliverable, and every build publishes a machine-readable inventory (SBOM)
of what the container image contains.

## ADDED Requirements

### Requirement: the runtime dependency graph allows only allowlisted licenses

The license policy check SHALL read the restored NuGet graph (each src/ project's
project.assets.json plus the local nuspec cache) and the frontend's package-lock.json,
and SHALL fail when any dependency carries a license token outside the allowlist. SPDX
expressions (OR/AND/WITH) SHALL be allowed when every alternative license is allowed.

#### Scenario: a dependency declares a disallowed license

- **WHEN** a package in the runtime graph declares a license outside the allowlist
  (e.g. GPL-3.0-only)
- **THEN** the license policy step fails, listing the offending package and license

#### Scenario: a package predates license expressions

- **WHEN** a nuspec carries no license expression (only a licenseUrl or an embedded
  license file)
- **THEN** the check fails loudly unless the package is explicitly mapped after reading
  its license, so nothing passes by silence

### Requirement: the container image ships with a published SBOM

The sbom CI job SHALL build the container image, scan it with Trivy (blocking on
HIGH/CRITICAL), and publish the Syft SPDX inventory as an artifact.

#### Scenario: image with a high-severity vulnerability

- **WHEN** the Trivy image scan reports a HIGH or CRITICAL vulnerability
- **THEN** the sbom job fails

#### Scenario: image build and SBOM publication

- **WHEN** the sbom job runs
- **THEN** syft produces a valid SPDX document for raccord:ci, uploaded as the
  `sbom.spdx.json` workflow artifact
