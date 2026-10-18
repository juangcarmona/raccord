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
<!-- pdac:cite id="UC-WSP-JOIN-WORKSPACE" digest="sha256:1e77d6ae23bb9ffd002ade2efb6ec362fc71cb1f10a3de666b288d32bed71aa7" -->
<!-- pdac:cite id="UC-WSP-CHANGE-PROFILE-EMAIL" digest="sha256:0fc2f64f44eb70c14f2dfb4bfc0e304a65079d86117f447c495355657084830a" -->
<!-- pdac:cite id="UC-CRW-MANAGE-CREW-LOGIN-ACCESS" digest="sha256:426f879ee3742eede9023e4cbf461c614f50ac8eb9eceec95147d29a7fc59dc7" -->
<!-- pdac:cite id="BR-WSP-ONE-PROFILE-PER-EMAIL" digest="sha256:93bc9f499b51fa1065d951dcfa00afb28c481a787f000765bf94411f973edeab" -->
<!-- pdac:cite id="BR-WSP-VERIFY-UNIQUE-IN-WORKSPACE" digest="sha256:e1f7f1b23072a7df82cbb1627611fd1848aaa7b28d6b68fb4143b569cc91c0b0" -->
<!-- pdac:cite id="BR-WSP-EMAIL-CHANGE-PROPAGATES" digest="sha256:df4a7ae92988bc3e0bbcd8da69dc0d2f66363a7e646b183d534117ac707c3b91" -->
<!-- pdac:cite id="BR-CRW-ACCOUNT-INVITATION-EXPIRY" digest="sha256:0b7b348c641e79d8d97ff46b53c5f869b08f5d042d360429810ee6cc67f3c5c1" -->
<!-- pdac:cite id="BR-CRW-PROFILE-OWNERSHIP" digest="sha256:8b7c288163c50c7481acce8b6d361ec6ff7cd1e5beea0018ee097e71e7fb1e5f" -->
<!-- pdac:cite id="QR-ACCESS-CONTROL" digest="sha256:71b7d808829672b6a0bd8d67df1d1748c5a143970b1030480b0d2351be15f0fb" -->