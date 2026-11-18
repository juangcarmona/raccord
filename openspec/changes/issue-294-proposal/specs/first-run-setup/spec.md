<!-- pdac-scope: cited -->

# Spec Delta

## Purpose

Let an installation's first administrator create the initial account through the browser without enabling repeated bootstrap or implying later company onboarding is complete.

## ADDED Requirements

### Requirement: Route visitors using authoritative setup status

The application MUST show setup to signed-out visitors only when the server reports no users. Initialized installations MUST offer sign-in instead. Unknown status MUST NOT enable setup. Signed-in users MUST retain access to the application.

#### Scenario: Fresh installation

- **WHEN** a signed-out visitor opens an installation with no users
- **THEN** the accessible setup screen is displayed

#### Scenario: Initialized or unknown status

- **WHEN** a visitor opens setup after initialization or status cannot be checked
- **THEN** account creation is unavailable and a sign-in recovery path is provided

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:bad3a5a4eae32e934a6ace45458a66c14eb33fbcfa02c2c1c22c7c3b922cc0fe" -->

### Requirement: Create the first administrator accessibly

The product MUST accept email and password through a labeled localized form, create the first account as Admin, and make Admin, Office, Warehouse and Crew available. Invalid submissions MUST show recoverable errors. Company details MUST NOT be prerequisites for first-account creation.

#### Scenario: Successful creation

- **WHEN** valid first-account credentials are submitted
- **THEN** one Admin exists, four roles are available and the visitor can continue to sign-in

#### Scenario: Validation or sign-in fails

- **WHEN** validation rejects a submission or sign-in fails after creation
- **THEN** the visitor receives a clear recovery path without automatically repeating account creation

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:bad3a5a4eae32e934a6ace45458a66c14eb33fbcfa02c2c1c22c7c3b922cc0fe" -->
<!-- pdac:cite id="BR-WSP-NEW-USER-DEFAULT-ROLE" digest="sha256:225d068645b0edea78a38030b719e72c188c78fcb75337160dde005db87e6c9c" -->
<!-- pdac:cite id="TERM-PREDEFINED-ROLE" digest="sha256:3e6f316bdfdb4e2c76eede9cbcf2db751b3806adad7f5397c15d27ffdff7fe16" -->

### Requirement: Warn about breached registration passwords privately

At registration the product MUST compare only a short password-hash fragment with public breach lists. A match MUST recommend a unique password without claiming an account breach or blocking sign-in. Lookup failure MUST remain distinct from a clear result and MUST NOT prevent account creation.

#### Scenario: Password match

- **WHEN** the chosen password matches a public breach entry
- **THEN** successful registration displays a warning recommending a unique password and permits sign-in

#### Scenario: Provider unavailable

- **WHEN** the breach service fails or times out
- **THEN** the account can still be created and the UI states the check was unavailable

<!-- pdac:cite id="BR-WSP-BREACHED-PASSWORD-CHECK" digest="sha256:8593dbb34d2881761fd788ab1ac34905d59270419aa1c918b0b84603658d6a34" -->
<!-- pdac:cite id="TERM-BREACHED-PASSWORD-WARNING" digest="sha256:de96c991d9ddb44461b4dc3518a17eb211fc432699cc687b245725f9842ef437" -->

### Requirement: Refuse repeat and competing setup

Once a user exists, the product MUST refuse any further first-account setup without creating another user. Concurrent initial requests MUST create at most one administrator. A stale browser form MUST communicate refusal and offer sign-in rather than another setup submission.

#### Scenario: Repeat or concurrent request

- **WHEN** another request has already created the initial user
- **THEN** setup is refused and the account count and four predefined roles remain unchanged

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:bad3a5a4eae32e934a6ace45458a66c14eb33fbcfa02c2c1c22c7c3b922cc0fe" -->
