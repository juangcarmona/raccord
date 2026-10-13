<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Authenticated web session

## ADDED Requirements

### Requirement: Restore and protect the web session

The web application MUST restore the current session through the typed API and navigate anonymous users to sign-in while leaving authorization to the server.

#### Scenario: Anonymous navigation is redirected

- **WHEN** an anonymous user opens a protected route
- **THEN** the browser navigates to sign-in and remembers the requested route

#### Scenario: Signed-in navigation is restored

- **WHEN** the current-user request succeeds
- **THEN** the protected application shell is rendered

### Requirement: Complete the session lifecycle

The web application MUST expose typed sign-in and sign-out actions with accessible pending, invalid-credentials and unavailable-server states.

#### Scenario: Sign-out returns to sign-in

- **WHEN** a signed-in user activates sign-out
- **THEN** the cookie session is cleared and the browser returns to sign-in
