<!-- pdac-scope: none reason="Repository engineering tooling and CI process; implements no product behaviour defined in docs/product/model." -->

# security-scanning Specification

## Purpose

Blocks a pull request that leaks a secret, carries a known-vulnerable dependency tree, or matches an insecure code pattern — three result-checking gates (TruffleHog, Trivy, Semgrep) that run on every PR alongside the deeper per-ecosystem audits (NuGet / npm).

## ADDED Requirements

### Requirement: secrets in git history block the build

TruffleHog SHALL scan the checked-out ref's full history and report findings newer than the PR base sha, and a finding SHALL fail the security job.

#### Scenario: a PR adds a hardcoded API key

- **WHEN** a commit in the PR range introduces a string matching a known credential format
- **THEN** the TruffleHog step exits non-zero and the security job fails

#### Scenario: a pre-existing unreported finding predates the base

- **WHEN** history before the PR base contains a TruffleHog finding that was not reported before
- **THEN** it is outside the base..head report window and does not fail this PR

### Requirement: insecure code patterns block the build

Semgrep SHALL run the in-repo `.semgrep/` rules layered with the p/owasp-top-ten and p/secrets registry sets, and a blocking finding SHALL fail the security job.

#### Scenario: Domain or Application constructs an HttpClient

- **WHEN** a C# file under src/Raccord.Domain or src/Raccord.Application contains `new HttpClient(...)`
- **THEN** the raccord-no-httpclient-in-core rule reports a blocking finding

#### Scenario: a credential-looking local variable is assigned a literal

- **WHEN** production code assigns a string literal to a variable whose name matches password/secret/token/apikey
- **THEN** the raccord-no-hardcoded-secrets rule reports a blocking finding

### Requirement: known vulnerabilities block the build

Trivy SHALL scan both the repository filesystem and the built container image for known vulnerabilities, and any HIGH or CRITICAL finding SHALL fail the security job.

#### Scenario: a transitive dependency ships a high-severity CVE

- **WHEN** the filesystem or image scan reports a HIGH or CRITICAL vulnerability
- **THEN** the corresponding Trivy step exits non-zero and the security job fails
