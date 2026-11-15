<!-- pdac-scope: cited -->

Use ASP.NET Core Identity with a `Guid` user key and the existing PostgreSQL context. Map Identity tables and data-protection keys to the `workspace` schema. Configure the application cookie as HttpOnly, SameSite=Lax and sliding, and override cookie redirects for API requests with status codes.

The API's `/api` route group requires authorization by default. Authentication endpoints explicitly opt out where necessary. The existing `ICurrentUser` adapter continues to read the authenticated principal's name, so audit records gain the signed-in account without a second actor abstraction.

The demo seed creates a documented fictional account only for the existing `migrate --seed-demo` path used by local Aspire E2E. First-run account creation is deliberately left to issue #39.

<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

<!-- pdac:cite id="TERM-USER-ACCOUNT" digest="sha256:e35b221d981e03d6c106ca25ff26ff507b9a616cd8e01ca80a9f6ee598b44d9c" -->

<!-- pdac:cite id="BR-WSP-BREACHED-PASSWORD-CHECK" digest="sha256:8593dbb34d2881761fd788ab1ac34905d59270419aa1c918b0b84603658d6a34" -->
