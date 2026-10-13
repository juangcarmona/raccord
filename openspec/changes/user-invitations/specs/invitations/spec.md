<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-JOIN-WORKSPACE" digest="sha256:1e77d6ae23bb9ffd002ade2efb6ec362fc71cb1f10a3de666b288d32bed71aa7" -->

# Invitations

## ADDED Requirements

### Requirement: Invitations expire and are single-use

An invitation MUST expire after 30 days, and accepting it MUST make the token unusable for all later attempts.

#### Scenario: Recipient replays an accepted link

- **WHEN** the recipient submits an invitation token that has already been accepted
- **THEN** the API rejects the request and does not create another user

### Requirement: Joining assigns the invited role

Accepting a valid invitation MUST create a local account for the invited email and assign the role selected by the administrator.

#### Scenario: Recipient chooses a password

- **WHEN** the recipient submits a valid token and a valid password
- **THEN** the account is created once and has the invited role
