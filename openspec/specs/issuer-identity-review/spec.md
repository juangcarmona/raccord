---
pdac-scope: cited
---

# Issuer legal-identity review Specification

## Purpose

Protects the issuer identity used for future invoices by requiring an authorized second administrator to review legal-name or tax-ID changes after invoicing begins.

## Requirements

### Requirement: Review legal-identity changes after invoicing begins

After any invoice has been issued, a change to issuer legal name or tax ID MUST remain unapplied until a distinct authorized administrator approves the request. Each request MUST contain a reason and proposed values. The reviewer MUST be able to inspect old values, proposed values, reason, and requester.

<!-- pdac:cite id="FR-ISSUER-001" digest="sha256:1d443dcade4a5af9dce47d75a383a243a298dd37de5a2d1be22bc80d5dfe1712" -->
<!-- pdac:cite id="UC-DEFINE-ISSUER-PROFILE" digest="sha256:fd058a02a9f6f90de16249ea291336c0193cfefce4236e0081a00f6da3223b34" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:5ac10d9c4ab5f68acd5a18213fe4c9c0d50786549a1d94c760409214b13355f7" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="CON-SINGLE-ISSUER" digest="sha256:f41d4ba8da941e762b3ee568fa15b478f1a60443cb2ea905d4826b13cdb1e1cd" -->

#### Scenario: Initial identity setup

- GIVEN no invoice has been issued
- WHEN an authorized administrator saves issuer identity
- THEN the identity is applied immediately using the existing settings flow

#### Scenario: Post-issuance identity change

- GIVEN an invoice has been issued
- WHEN an administrator changes legal name or tax ID with a reason
- THEN the change is recorded as pending and the active issuer identity is unchanged

#### Scenario: Missing reason

- GIVEN an invoice has been issued
- WHEN an administrator submits a legal-identity change without a non-empty reason
- THEN the request is rejected and the active identity remains unchanged

#### Scenario: Authorized second-person approval

- GIVEN a pending request created by one administrator
- WHEN a different administrator with issuer-settings permission approves it
- THEN only future invoices use the accepted identity and the decision is audited

#### Scenario: Self-approval or unauthorized decision

- GIVEN a pending request
- WHEN its requester attempts to approve it, or a user without issuer-settings permission attempts a decision
- THEN the request remains pending and the active identity remains unchanged

#### Scenario: Rejection and abandoned request

- GIVEN a pending request
- WHEN an authorized distinct administrator rejects it or it is left undecided
- THEN the active identity remains unchanged and request history remains retained and auditable

#### Scenario: Concurrent decisions

- GIVEN a pending request
- WHEN two reviewers attempt to decide it concurrently
- THEN at most one decision is applied and recorded as successful

#### Scenario: Only one open request

- GIVEN an issuer already has a pending identity request
- WHEN an administrator submits another legal-identity request for that issuer
- THEN the second request is rejected and the original request remains pending

#### Scenario: Address and contact changes

- GIVEN an issuer profile and any invoice history
- WHEN an authorized administrator changes only address or contact details
- THEN the change applies immediately to future invoices without a second-person review

#### Scenario: Issued invoice snapshots

- GIVEN an invoice issued before an approved identity change
- WHEN the issuer profile changes
- THEN the previously issued invoice and stored representation remain unchanged
