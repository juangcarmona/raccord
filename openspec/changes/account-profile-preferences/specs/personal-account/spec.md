<!-- pdac-scope: cited -->

# Personal account

<!-- pdac:cite id="UC-WSP-EDIT-PROFILE" digest="sha256:110a8e418914abd5f8a156d5729ec9a0055972823b845b5d17c5e1873d6b99ae" -->
<!-- pdac:cite id="UC-WSP-SET-LANGUAGE" digest="sha256:4e1830ebc5acc09bdedf5900f8543f6d29ee5a13439f608dfb055a43bf972aed" -->
<!-- pdac:cite id="FR-WSP-016" digest="sha256:a6318f734164595fecbe906b6be4b7e17830d40f1e32d1db2fc161f5a3ad63d5" -->
<!-- pdac:cite id="BR-WSP-PROFILE-OWNED-BY-PERSON" digest="sha256:cc05019851b877d398a0a749334a59b40bc905731e3744f87b67532ba493f7c6" -->
<!-- pdac:cite id="BR-WSP-LANGUAGE-SCOPES" digest="sha256:495045d198c600a95725b0796363ad8d29d8690f67b494c4b12be5275178b335" -->
<!-- pdac:cite id="TERM-PERSONAL-PROFILE" digest="sha256:9e904c99957e8115e03adbe906f242aae13fa65eb1dea0e1a4f7bf1667d9675c" -->
<!-- pdac:cite id="TERM-PROFILE-LANGUAGE" digest="sha256:0782e7729b035f366560a1116c249189a7e896cf146614e2f44bf3078889acd8" -->

## ADDED Requirements

### Requirement: Users can inspect and maintain their own basic account profile

The product MUST provide an authenticated account page that displays the current user's sign-in email and lets that user maintain their own display name and supported profile language. The sign-in email MUST remain read-only in this capability. A profile update MUST be scoped to the authenticated user and MUST NOT accept a target user identifier from the client.

#### Scenario: User opens their account page

- **WHEN** a signed-in user follows the account entry in the application header
- **THEN** the account page shows their sign-in email, current display name, and current personal language preference

#### Scenario: User saves their own profile

- **WHEN** a signed-in user submits a valid display name and supported language
- **THEN** only that user's profile is updated and the saved values are returned

#### Scenario: User submits invalid profile values

- **WHEN** a signed-in user submits a blank or overlong display name, or an unsupported language
- **THEN** the request is rejected with a field-level validation response and no profile data changes

#### Scenario: Unauthenticated caller tries to read or update a profile

- **WHEN** a caller without a signed-in session reads or updates the account profile
- **THEN** the API returns HTTP 401 and no profile data changes

### Requirement: Personal language is distinct from workspace language

The product MUST apply the saved profile language to the signed-in user's interface without changing the workspace language, other users' preferences, or workspace-level notifications and defaults. A user's saved language MUST be loaded from the server when their authenticated session is established; local storage MAY be used only as a display fallback while the server value loads.

#### Scenario: User changes personal language

- **WHEN** the user saves another supported profile language
- **THEN** their current interface changes to that language and the preference remains after sign-out and sign-in

#### Scenario: Personal language change does not affect colleagues or workspace settings

- **WHEN** one user changes their profile language
- **THEN** another user's language and the workspace language remain unchanged

### Requirement: Header account entry respects role-management permissions

The application header MUST provide an account entry that identifies the signed-in user and a direct sign-out action. The account page MAY link to existing role administration only when the current user has the corresponding role-management permission; the server MUST continue to enforce authorization for the role API.

#### Scenario: User with role-management permission sees role administration link

- **WHEN** a signed-in user with role-management permission opens their account page
- **THEN** the page provides a link to the existing roles and permissions page

#### Scenario: User without role-management permission does not see role administration link

- **WHEN** a signed-in user without role-management permission opens their account page
- **THEN** the page does not provide the administrative role-management link

<!-- pdac:cite id="FR-WSP-018" digest="sha256:38cd7c21cde0b0d0037f1475e74e69dd454a153b247d804ff2f862bfa1d0db6d" -->
<!-- pdac:cite id="SB-WSP-PROFILE-LANGUAGE-ONLY-SCREENS" digest="sha256:d487ec2e88e21259cce5eb9b05dc6f1899a27175c4d31b753f73aab32d118c4c" -->
<!-- pdac:cite id="ACT-CREW-MEMBER" digest="sha256:7e8350f158cc4ac6c30857753bcf74655a043878f28b0d5561343b7092ecfdbe" -->
<!-- pdac:cite id="BC-WORKSPACE" digest="sha256:013841dd37789974afef821eb2c071a85acaa72c083abd2285b7c406db95f8a5" -->
