<!-- pdac-scope: cited -->

# Crew account association invitations

## What Changes

- Add the Crew account-management permission and use it to protect record-specific association invitations.
- Send a single-use 30-day invitation through operator-configured SMTP; store only a token digest.
- Let a verified, authenticated profile holder accept or reject the specific existing crew record invitation.
- Enforce one-to-one profile and crew associations and audit outcomes.
- Add focused planner and acceptance screens in English and Spanish, with a Workspace/Crew Aspire browser journey and context-owned evidence.

## Capabilities

### New Capabilities

- `crew-account-association`: Issue, deliver and accept an invitation that associates one existing profile with one existing crew record.

### Modified Capabilities

- None. The permission is part of the new invitation capability and is exposed through the existing Workspace role API.

## Impact

Workspace roles and current-user API; Crew and Workspace persistence; account invitation email; authenticated SPA routes; Aspire browser evidence. MailKit is added as an SMTP adapter behind an application-owned email port under ADR-0028. Unlinking established associations, account deactivation and email-change propagation remain a separate later implementation slice.

<!-- pdac:cite id="BR-WSP-ACCOUNT-CREW-ASSOCIATION" digest="sha256:1452caa625c12626db7880c8623ed9c726c4f914879a3385a73b2f4a3c9a2966" -->
<!-- pdac:cite id="UC-WSP-LINK-EXISTING-CREW-RECORD" digest="sha256:641960ffc7fd8228156b926012c0ab2edae42b2e0e4fbb4111e1c52e44635462" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-LINKED" digest="sha256:7c138d265a2ebf6645c469fe56442712482a76b5a05cef0f7a3faaf4168bb588" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-AMBIGUOUS" digest="sha256:9ecc890588c04cc82326869f03ced14090910fee52a445c756a1274717b5071d" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:2bc8d3c16cae9e821f1aa0297f7025097b9163a6e22fe04599402a904fc59d3d" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->
