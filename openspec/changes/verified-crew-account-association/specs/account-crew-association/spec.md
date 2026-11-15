<!-- pdac-scope: cited -->

# Account to crew association

## Purpose

Define product behavior for associating an existing personal profile with an existing crew record while preserving them as distinct records.

## ADDED Requirements

### Requirement: The person confirms a planner-selected crew record

An authorized planner MAY invite a person to associate an existing profile with one selected crew record. The association SHALL take effect only after the authenticated person explicitly accepts the record-specific invitation.

#### Scenario: Person accepts selected association

- **GIVEN** the planner has permission and selects an unlinked crew record
- **AND** the invitation is current and the person has authenticated to a personal profile
- **WHEN** the person reviews and accepts the invitation for that record
- **THEN** the product associates that profile with the selected crew record
- **AND** records the actor, time, workspace, affected record identifiers and outcome
- **AND** preserves both records and their histories

### Requirement: Email equality alone never proves identity

The product SHALL NOT create or prove an association solely because email addresses match.

#### Scenario: Matching email addresses

- **GIVEN** a profile and one or more crew records have matching email addresses
- **WHEN** the product evaluates a requested association
- **THEN** it SHALL require explicit acceptance of a record-specific invitation
- **AND** SHALL NOT associate records automatically

### Requirement: Ambiguous and conflicting records are refused

The product SHALL refuse an association if no plausible crew record exists, more than one plausible record exists, or either record is already associated with another record of the opposite type.

#### Scenario: Candidate is ambiguous or already linked

- **GIVEN** the request has zero or multiple plausible crew records, or either record is already linked elsewhere
- **WHEN** the product evaluates the association
- **THEN** it SHALL create no link and preserve both records
- **AND** require authorized planner resolution without disclosing another workspace's data
- **AND** audit the refusal and reason

### Requirement: Association changes are authorized and audited

The product SHALL authorize each create or remove action within its workspace and audit successful, rejected and removed attempts with actor, time, workspace, affected record identifiers, outcome and reason.

#### Scenario: Unauthorized request

- **GIVEN** an actor lacks permission to manage crew login access
- **WHEN** the actor attempts to create or remove an association
- **THEN** the product SHALL refuse without changing either record or disclosing other-workspace data

#### Scenario: Planner removes a link

- **GIVEN** an association exists and an authorized planner has a reason to remove it
- **WHEN** the planner removes the link and records that reason
- **THEN** the association is removed and no longer grants access
- **AND** both records and their histories are preserved

### Requirement: Email changes and deactivation preserve association semantics

A verified profile-email change SHALL update the linked crew record under the existing email-propagation rule without changing the association. Deactivating crew login access SHALL block sign-in while preserving the association and crew history.

#### Scenario: Verified email changes

- **GIVEN** a profile is associated with a crew record
- **WHEN** its owner verifies a new profile email
- **THEN** the new address propagates to the linked crew record
- **AND** the association remains unchanged

#### Scenario: Crew login is deactivated

- **GIVEN** a profile is associated with a crew record
- **WHEN** an authorized planner deactivates login access
- **THEN** the person cannot sign in through that crew association
- **AND** the association and crew history remain

<!-- pdac:cite id="TERM-USER-ACCOUNT" digest="sha256:e35b221d981e03d6c106ca25ff26ff507b9a616cd8e01ca80a9f6ee598b44d9c" -->
<!-- pdac:cite id="TERM-PERSONAL-PROFILE" digest="sha256:9e904c99957e8115e03adbe906f242aae13fa65eb1dea0e1a4f7bf1667d9675c" -->
<!-- pdac:cite id="TERM-CREW-MEMBER" digest="sha256:be1b9db1fba459e769e76964f44ebf5b4dc94b31f75ee257b060b6f2604b492e" -->
<!-- pdac:cite id="UC-WSP-JOIN-WORKSPACE" digest="sha256:ae1594869df4c3b607cafdc77c74c5d57b2de52e8dd99eae6e2205d208c29006" -->
<!-- pdac:cite id="UC-WSP-CHANGE-PROFILE-EMAIL" digest="sha256:71ae8f6ebb91255f46137e0fe07ff846148e44b461c645037309d0a5522a09f1" -->
<!-- pdac:cite id="UC-CRW-MANAGE-CREW-LOGIN-ACCESS" digest="sha256:6dbdc959fe855ef60bc10f8927da76ec056e78a6af72464f45266f69daba51dc" -->
<!-- pdac:cite id="BR-WSP-ONE-PROFILE-PER-EMAIL" digest="sha256:c7a7278ed5c748d335a0c36c81d74ae0c34f6820aba18c7a8b2a338c006e82eb" -->
<!-- pdac:cite id="BR-WSP-VERIFY-UNIQUE-IN-WORKSPACE" digest="sha256:e1f7f1b23072a7df82cbb1627611fd1848aaa7b28d6b68fb4143b569cc91c0b0" -->
<!-- pdac:cite id="BR-WSP-EMAIL-CHANGE-PROPAGATES" digest="sha256:be0bcbb9c0d4119d48695fef7d096019c945ae4999809937ac59c131c554597b" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:2bc8d3c16cae9e821f1aa0297f7025097b9163a6e22fe04599402a904fc59d3d" -->
<!-- pdac:cite id="BR-CRW-PROFILE-OWNERSHIP" digest="sha256:f4202845b7649678220c75b4f144a4be80467e72a5eec42ccc5a1e16ec52ea3b" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->

<!-- pdac:cite id="BR-WSP-ACCOUNT-CREW-ASSOCIATION" digest="sha256:1452caa625c12626db7880c8623ed9c726c4f914879a3385a73b2f4a3c9a2966" -->
<!-- pdac:cite id="UC-WSP-LINK-EXISTING-CREW-RECORD" digest="sha256:641960ffc7fd8228156b926012c0ab2edae42b2e0e4fbb4111e1c52e44635462" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-AMBIGUOUS" digest="sha256:9ecc890588c04cc82326869f03ced14090910fee52a445c756a1274717b5071d" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-DEACTIVATED" digest="sha256:ec4c684e6f4ce14813bb8ce452adfeb0c5288d7301109b8317b4e869b608275a" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-EMAIL-CHANGED" digest="sha256:f1628aa94a1727c8fd121388b852832b29173a2c26fc44538e7fe8989f11e04d" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-LINKED" digest="sha256:7c138d265a2ebf6645c469fe56442712482a76b5a05cef0f7a3faaf4168bb588" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-UNAUTHORIZED" digest="sha256:01d289f5f54c52b4a8cf304cce91b6a4417dbcfbcf46aa0cc9497ed337badde3" -->
<!-- pdac:cite id="SB-WSP-ACCOUNT-CREW-UNLINKED" digest="sha256:30deb10131507d49ebd05c23ec44891ac860fb8cfafc544b14369db3fe1fa572" -->
