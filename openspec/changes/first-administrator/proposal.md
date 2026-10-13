<!-- pdac-scope: cited -->

## Why

A fresh installation needs a safe first-run boundary before authenticated work can begin. The first person must become the administrator, and the installation must not offer a second setup path after that account exists.

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:e5ee7da50f5c081b1c257461484a9e6eecfbce0f108a5abd2d388c14c8e4f16a" -->

<!-- pdac:cite id="TERM-PREDEFINED-ROLE" digest="sha256:40c8223391cb84a39ccfa1047f4cec3e38063e401caf61b52a31a2d14d69e7c5" -->

## What changes

- Provide anonymous setup status and first-administrator endpoints.
- Create the first local user and assign the Admin role in one transaction.
- Seed Admin, Office, Warehouse and Crew roles for the installation.
- Provide a setup page and exercise it through the real Aspire browser journey.
- Keep company details, sample data and editable permissions for later child issues.

## Impact

The web host, Workspace Identity persistence, SPA routes, AppHost E2E bootstrap and API contract change. The E2E environment no longer seeds demo users or projects, so journeys verify setup rather than relying on preloaded state.
