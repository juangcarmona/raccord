<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: API access requires a signed-in local account

The web API MUST require an authenticated local user and MUST return 401 for an unauthenticated request instead of redirecting to an HTML sign-in page.

#### Scenario: Anonymous API request

- **WHEN** a client requests a protected API route without the application cookie
- **THEN** the response status is 401

### Requirement: Local sign-in uses a durable cookie session

The application MUST accept a valid local email and password, issue an HttpOnly same-origin cookie, and persist data-protection keys so the cookie remains valid after an application restart.

#### Scenario: Valid credentials

- **WHEN** a user submits valid local credentials
- **THEN** the response succeeds and the browser receives an HttpOnly application cookie

#### Scenario: Repeated invalid credentials

- **WHEN** a user repeatedly submits invalid passwords
- **THEN** the account is locked according to the configured lockout policy

<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

<!-- pdac:cite id="TERM-USER-ACCOUNT" digest="sha256:e35b221d981e03d6c106ca25ff26ff507b9a616cd8e01ca80a9f6ee598b44d9c" -->
