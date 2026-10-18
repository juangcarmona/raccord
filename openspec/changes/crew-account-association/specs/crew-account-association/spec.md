<!-- pdac-scope: cited -->

# Crew account association

## Purpose

Implement the approved record-specific invitation and authenticated confirmation flow between an existing Crew member and an existing Workspace profile.

## ADDED Requirements

### Requirement: Only authorized planners manage account-link invitations

The product SHALL require the `crew:accounts.manage` permission to issue, resend or manage an invitation. The Crew role SHALL not receive this permission by default.

#### Scenario: Planner without permission is refused

- **GIVEN** the signed-in actor does not have `crew:accounts.manage`
- **WHEN** the actor requests an account-link invitation
- **THEN** the product SHALL return forbidden
- **AND** SHALL leave invitation and association state unchanged
- **AND** SHALL not disclose another profile's data

### Requirement: Invitation is bound to one record and expires

An invitation SHALL identify exactly one existing Crew record, be delivered only to its current email address, expire after 30 days and be usable at most once. The product SHALL store only a digest of the random token.

#### Scenario: Planner sends invitation

- **GIVEN** an authorized planner selects an unlinked Crew record with a usable email
- **WHEN** the planner sends a profile-link invitation
- **THEN** the product SHALL send a record-specific, 30-day invitation to that email
- **AND** store no raw token in persistence, audit or logs

#### Scenario: Email delivery fails

- **GIVEN** an authorized planner issues an invitation and SMTP is unavailable or unconfigured
- **WHEN** delivery fails
- **THEN** the UI SHALL report a retryable failure
- **AND** no profile-to-crew association SHALL be created
- **AND** a resend SHALL rotate the invitation token safely

### Requirement: Authenticated profile holder confirms association

Only the authenticated holder of a verified profile whose email matches the invitation target MAY accept the invitation. Email equality without explicit acceptance SHALL NOT create an association.

#### Scenario: Person accepts the selected record

- **GIVEN** the invitation is current and the signed-in profile has the same verified email as its target
- **WHEN** the person reviews and accepts the invitation for the named Crew record
- **THEN** the product SHALL associate that profile and record
- **AND** record the result in the audit trail
- **AND** preserve both records and their histories

#### Scenario: Person rejects or has a different email

- **GIVEN** the invitation is current
- **WHEN** its recipient rejects it, or a signed-in profile with another email attempts acceptance
- **THEN** rejection SHALL create no link
- **AND** the mismatched profile SHALL be refused without disclosing the target profile

### Requirement: Association is one-to-one and atomic

The product SHALL permit at most one Crew record per personal profile and one profile per Crew record in this single-organization installation. Acceptance SHALL consume the invitation and create the association atomically.

#### Scenario: Another request links one of the records first

- **GIVEN** an invitation is pending and another request has linked its profile or Crew record
- **WHEN** the pending invitation is accepted
- **THEN** the product SHALL refuse the conflict
- **AND** preserve the existing association and both records
- **AND** audit the conflict without merging or deleting records

#### Scenario: Two acceptances race

- **GIVEN** two invitations are accepted concurrently for a profile or Crew record
- **WHEN** both transactions attempt to create a link
- **THEN** at most one association SHALL commit
- **AND** the losing attempt SHALL receive a conflict and create no second link

<!-- pdac:cite id="BR-WSP-ACCOUNT-CREW-ASSOCIATION" digest="sha256:7b1c7b7971ae29810b6f4999edf899302fd7fa738a8bc2b7cbf2d6d8ad70ee6f" -->
<!-- pdac:cite id="UC-WSP-LINK-EXISTING-CREW-RECORD" digest="sha256:641960ffc7fd8228156b926012c0ab2edae42b2e0e4fbb4111e1c52e44635462" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-LINKED" digest="sha256:7c138d265a2ebf6645c469fe56442712482a76b5a05cef0f7a3faaf4168bb588" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-AMBIGUOUS" digest="sha256:9ecc890588c04cc82326869f03ced14090910fee52a445c756a1274717b5071d" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:0b7b348c641e79d8d97ff46b53c5f869b08f5d042d360429810ee6cc67f3c5c1" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->
