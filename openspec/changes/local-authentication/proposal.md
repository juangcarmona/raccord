<!-- pdac-scope: cited -->

## Why

The application currently attributes every request to an unauthenticated actor and exposes the product API without a sign-in boundary. This change starts epic #17 with local accounts, same-origin cookie sessions, and a sign-in surface.

<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

<!-- pdac:cite id="TERM-USER-ACCOUNT" digest="sha256:e35b221d981e03d6c106ca25ff26ff507b9a616cd8e01ca80a9f6ee598b44d9c" -->

## What changes

- Store local user accounts and data-protection keys in the Workspace schema.
- Authenticate with an HttpOnly same-origin cookie and return 401/403 from API routes instead of redirects.
- Lock accounts after repeated failed password attempts.
- Provide sign-in and sign-out endpoints and the SPA sign-in page.
- Keep first-run account creation, roles, permissions, external sign-in and password recovery for the ordered child issues of epic #17.

## Impact

The web host, Workspace persistence, API authorization boundary, frontend routes, migrations and E2E login setup change. Existing web tests use an explicit test-only bypass while the Aspire E2E suite signs in through the real cookie flow.
